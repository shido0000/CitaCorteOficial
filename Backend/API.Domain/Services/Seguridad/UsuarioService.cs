using API.Data.Dtos.UsuarioDto;
using API.Data.Entidades.Barbers;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Barbers;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Web.Helpers;

namespace API.Domain.Services.Seguridad
{
    public class UsuarioService : BasicService<Usuario, UsuarioValidator>, IUsuarioService
    {
        private ISolicitudDeSuscripcionService _SolicitudDeSuscripcionService;
        public UsuarioService(IUnitOfWork<Usuario> repositorios, IHttpContextAccessor httpContext, ISolicitudDeSuscripcionService solicitudDeSuscripcionService) : base(repositorios, httpContext)
        {
            _SolicitudDeSuscripcionService = solicitudDeSuscripcionService;
        }

        public override async Task<EntityEntry<Usuario>> Crear(Usuario entity)
        {

            //encriptando contraseña
            //HashPassword genera una cadena aleatoria en cada llamada (aunque se le pase el mismo texto)
            //para comprobar el pass usar la funcion VerifyHashedPassword pasandole por parametros
            //el texto generado y el texto plano a verificar

            entity.Contrasenna = Crypto.HashPassword(entity.Contrasenna);

            await ValidarAntesCrear(entity);

            return await _repositorios.BasicRepository.AddAsync(base.EstablecerDatosAuditoria(entity));

        }

        public override async Task<EntityEntry<Usuario>> Actualizar(Usuario entity)
        {
            Usuario? usuario = await ObtenerPorId(entity.Id) ??
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

            entity.Contrasenna = usuario.Contrasenna;
            entity.DebeCambiarContrasenna = usuario.DebeCambiarContrasenna;

            return await base.Actualizar(entity);
        }

        public async Task CambiarContrasenna(Guid usuarioId, string nuevaContrasenna, bool debeCambiarContrasenna = false)
        {
            Usuario? usuario = await ObtenerPorId(usuarioId) ??
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

            usuario.Contrasenna = Crypto.HashPassword(nuevaContrasenna);
            usuario.DebeCambiarContrasenna = debeCambiarContrasenna;

            await base.Actualizar(usuario);
        }


        public override async Task ValidarAntesActualizar(Usuario usuario)
        {
            ///validando los datos insertados por el usuario
            await new UsuarioValidator(_repositorios).ValidateAndThrowAsync(usuario);

            ///validando reglas de negocios    

            //se debe validar que el elemento exista antes de actualizar,
            //porque si el elemento no existe porque fue eliminado entonces lo crea
            //(aqui esta comentado porque en el metodo de Actualizar ya se hace esta validacion para el caso de los usuarios)

            //if (!await _repositorios.BasicRepository.AnyAsync(e => e.Id == entity.Id))
            //    throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
        }
        public async Task<Usuario?> ObtenerPorUsername(string username, Func<IQueryable<Usuario>, IIncludableQueryable<Usuario, object>>? propiedadesIncluidas = null) => await _repositorios.BasicRepository.FirstAsync(entity => entity.Username == username, propiedadesIncluidas);

        public async Task<List<Permiso>> ObtenerPermisos(string username)
            => (await _repositorios.Usuarios.FirstAsync(e => e.Username == username, query => query.Include(e => e.Rol.RolPermiso).ThenInclude(e => e.Permiso)))?.Rol.RolPermiso.Select(e => e.Permiso).ToList() ?? new();

        public async Task<Guid> CrearUsusarioMejorado(UsuarioNuevoDto usuario)
        {
            var nuevoUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Contrasenna = Crypto.HashPassword(usuario.Contrasenna),
                Correo = usuario.Correo,
                Apellidos = usuario.Apellidos,
                Nombre = usuario.Nombre,
                RolId = usuario.RolId,
                Username = usuario.Username,
                DebeCambiarContrasenna = false
            };

            await new UsuarioValidator(_repositorios).ValidateAndThrowAsync(nuevoUsuario);
            await _repositorios.Usuarios.AddAsync(nuevoUsuario);

            var tipoUsuario = await _repositorios.Roles
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.Id == nuevoUsuario.RolId)
                                    .Select(e => e.Nombre)
                                    .FirstOrDefaultAsync();

            var suscripcionFreeBarberoId = await _repositorios.Suscripciones
                        .GetQuery()
                        .AsNoTracking()
                        .Where(e => e.NivelSuscripcion == NivelSuscripcion.Free)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();

            if (tipoUsuario == "Barbero")
            {
                var nuevoBarbero = new Barbero
                {
                    Id = Guid.NewGuid(),
                    EstaAfiliadoABarberia = false,
                    BarberiaId = null,
                    UsuarioId = nuevoUsuario.Id,
                    SuscripcionId = suscripcionFreeBarberoId,
                    EstadoSuscripcion = EstadoSuscripcion.Aprobada,
                    Direccion = usuario.Direccion,
                    LogoUrl = usuario.LogoUrl,
                    HorarioApertura = "9:00",
                    HorarioCierre = "18:00"
                };
                await _repositorios.Barberos.AddAsync(nuevoBarbero);
            }
            else if (tipoUsuario == "Barberia")
            {
                var nuevoBarberia = new Barberia
                {
                    Id = Guid.NewGuid(),
                    Nombre = usuario.NombreBarberia,
                    Direccion = usuario.Direccion,
                    Telefono = usuario.Telefono,
                    LogoUrl = usuario.LogoUrl,
                    EstadoSuscripcion = EstadoSuscripcion.Pendiente,
                    SuscripcionId = usuario.SuscripcionId,
                    UsuarioId = nuevoUsuario.Id,
                    HorarioApertura = "9:00",
                    HorarioCierre = "18:00"
                };
                await _repositorios.Barberias.AddAsync(nuevoBarberia);
                await _SolicitudDeSuscripcionService.SolicitarNuevaSuscripcion(usuario.SuscripcionId.Value, nuevoBarberia.Id, null);
            }
            else if (tipoUsuario == "Cliente")
            {
                var nuevoCliente = new Cliente
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = nuevoUsuario.Id,
                };
                await _repositorios.Clientes.AddAsync(nuevoCliente);
            }
            else
            {
                var nuevoComercial = new Comercial
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = nuevoUsuario.Id,
                };
                await _repositorios.Comerciales.AddAsync(nuevoComercial);
            }

            await _repositorios.BasicRepository.SaveChangesAsync();
            return nuevoUsuario.Id;
        }

        public async Task<UsuarioPerfilDto> ObtenerDatosUsuario(string username)
        {
            return await _repositorios.Usuarios
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Rol)
                                    .Include(e => e.Barberia)
                                    .Include(e => e.Barbero)
                                    .Include(e => e.Cliente)
                                    .Where(e => e.Username == username)
                                    .Select(e => new UsuarioPerfilDto
                                    {
                                        Id = e.Id,
                                        RolId = e.RolId,
                                        NombreRol = e.Rol.Nombre,
                                        Nombre = e.Nombre,
                                        Apellidos = e.Apellidos,
                                        Username = e.Username,
                                        Correo = e.Correo,
                                        NombreBarberia = e.Barberia != null ? e.Barberia.Nombre : null,
                                        Direccion = e.Barberia != null ? e.Barberia.Direccion : e.Barbero != null ? e.Barbero.Direccion : null,
                                        Telefono = e.Barberia != null ? e.Barberia.Telefono : e.Barbero != null ? e.Barbero.Telefono : null,
                                        LogoUrl = e.Barberia != null ? e.Barberia.LogoUrl : e.Barbero != null ? e.Barbero.LogoUrl : null,
                                        SuscripcionId = e.Barberia != null ? e.Barberia.SuscripcionId : e.Barbero != null ? e.Barbero.SuscripcionId : null,
                                    })
                                    .FirstOrDefaultAsync()
                                   ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
        }
        public async Task<UsuarioPerfilDto> ObtenerDatosUsuarioPorId(Guid id)
        {
            return await _repositorios.Usuarios
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Rol)
                                    .Include(e => e.Barberia)
                                    .Include(e => e.Barbero)
                                    .Include(e => e.Cliente)
                                    .Where(e => e.Id == id)
                                    .Select(e => new UsuarioPerfilDto
                                    {
                                        Id = e.Id,
                                        RolId = e.RolId,
                                        NombreRol = e.Rol.Nombre,
                                        Nombre = e.Nombre,
                                        Apellidos = e.Apellidos,
                                        Username = e.Username,
                                        Correo = e.Correo,
                                        NombreBarberia = e.Barberia != null ? e.Barberia.Nombre : null,
                                        Direccion = e.Barberia != null ? e.Barberia.Direccion : e.Barbero != null ? e.Barbero.Direccion : null,
                                        Telefono = e.Barberia != null ? e.Barberia.Telefono : e.Barbero != null ? e.Barbero.Telefono : null,
                                        LogoUrl = e.Barberia != null ? e.Barberia.LogoUrl : e.Barbero != null ? e.Barbero.LogoUrl : null,
                                        SuscripcionId = e.Barberia != null ? e.Barberia.SuscripcionId : e.Barbero != null ? e.Barbero.SuscripcionId : null,
                                    })
                                    .FirstOrDefaultAsync()
                                   ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
        }


        public async Task<Guid> ActualizarUsusarioMejorado(UsuarioActualizarDto user)
        {
            var usuario = await _repositorios.Usuarios
                                    .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.Id == user.Id);

            if (!string.IsNullOrEmpty(user.Contrasenna))
            {
                usuario.Contrasenna = Crypto.HashPassword(user.Contrasenna);
            }
            usuario.Correo = user.Correo;
            usuario.Apellidos = user.Apellidos;
            usuario.Nombre = user.Nombre;
            usuario.Username = user.Username;

            await new UsuarioValidator(_repositorios).ValidateAndThrowAsync(usuario);
            _repositorios.Usuarios.Update(usuario);

            var tipoUsuario = await _repositorios.Roles
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.Id == usuario.RolId)
                                    .Select(e => e.Nombre)
                                    .FirstOrDefaultAsync();

            if (tipoUsuario == "Barbero")
            {
                var barbero = await _repositorios.Barberos
                                     .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.UsuarioId == user.Id);

                barbero.Direccion = user.Direccion;
                barbero.LogoUrl = user.LogoUrl;
                barbero.Telefono = user.Telefono;
                barbero.HorarioApertura = user.HorarioApertura;
                barbero.HorarioCierre = user.HorarioCierre;

                _repositorios.Barberos.Update(barbero);
            }
            else if (tipoUsuario == "Barberia")
            {
                var barberia = await _repositorios.Barberias
                                     .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.UsuarioId == user.Id);

                barberia.Direccion = user.Direccion;
                barberia.LogoUrl = user.LogoUrl;
                barberia.Telefono = user.Telefono;
                barberia.Nombre = user.NombreBarberia;
                barberia.HorarioApertura = user.HorarioApertura;
                barberia.HorarioCierre = user.HorarioCierre;

                _repositorios.Barberias.Update(barberia);
            }
            await _repositorios.BasicRepository.SaveChangesAsync();
            return usuario.Id;
        }

        public async Task<bool> CambiarEstadoUsuario(Guid id)
        {
            var usuario = await _repositorios.Usuarios
                                    .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.Id == id)
                                    ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
            if (usuario.Activo)
            {
                usuario.Activo = false;
            }
            else
            {
                usuario.Activo = true;
            }

            _repositorios.Usuarios.Update(usuario);
            await _repositorios.BasicRepository.SaveChangesAsync();
            return usuario.Activo;
        }
    }
}