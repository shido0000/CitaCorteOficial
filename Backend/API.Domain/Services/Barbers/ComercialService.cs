using API.Data.Dtos.ComercialDto;
using API.Data.Dtos.SuscripcionDto;
using API.Data.Entidades.Barbers;
using API.Data.Enum;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Barbers
{
    public class ComercialService : BasicService<Comercial, ComercialValidator>, IComercialService
    {

        public ComercialService(IUnitOfWork<Comercial> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }
        public async Task<int> TotalBarberos()
        {
            return await _repositorios.Barberos
                                    .GetQuery()
                                    .AsNoTracking()
                                    .CountAsync();
        }
        public async Task<int> TotalBarberias()
        {
            return await _repositorios.Barberias
                                    .GetQuery()
                                    .AsNoTracking()
                                    .CountAsync();
        }

        public async Task<int> TotalSuscripcionesVencidas()
        {
            var hoy = DateTime.Today;
            int cantidadSuscripcionesVencidasBarberias = await _repositorios.Barberias
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.FechaVencimientoSuscripcion.HasValue && e.FechaVencimientoSuscripcion.Value.Date < hoy)
                                    .CountAsync();

            int cantidadSuscripcionesVencidasBarberos = await _repositorios.Barberos
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.FechaVencimientoSuscripcion.HasValue && e.FechaVencimientoSuscripcion.Value.Date < hoy)
                                    .CountAsync();

            return cantidadSuscripcionesVencidasBarberias + cantidadSuscripcionesVencidasBarberos;
        }
        public async Task<List<SolicitudSuscripcionDto>> ObtenerListaSolicitudesSuscripcion()
        {
            var solicitudes = await _repositorios.SolicitudDeSuscripciones
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Barberia)
                                        .ThenInclude(e => e.Usuario)
                                    .Include(e => e.Barbero)
                                        .ThenInclude(e => e.Usuario)
                                    .Where(e => e.EstadoSuscripcion.HasValue && e.EstadoSuscripcion.Value == EstadoSuscripcion.Pendiente)
                                    .ToListAsync();
            var suscripciones = await _repositorios.Suscripciones
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Select(e => new
                                    {
                                        Id = e.Id,
                                        Nombre = e.Nombre,
                                    })
                                    .ToListAsync();

            List<SolicitudSuscripcionDto> listaRetornar = new();
            foreach (var solicitud in solicitudes)
            {
                var suscripcionAnteriorId = solicitud.BarberoId != null ? solicitud.Barbero.SuscripcionId : solicitud.BarberiaId != null ? solicitud.Barberia.SuscripcionId : Guid.Empty;

                var nuevaSoli = new SolicitudSuscripcionDto
                {
                    Nombre = solicitud.BarberoId != null ? solicitud.Barbero.Usuario.NombreCompleto : solicitud.BarberiaId != null ? solicitud.Barberia.Nombre : "-",
                    Estado = solicitud.EstadoSuscripcion?.ToString() ?? EstadoSuscripcion.Pendiente.ToString(),
                    FechaSolicitud = solicitud.FechaSolicitado?.ToString("dd/MM/yyyy"),
                    SuscripcionNuevaId = solicitud.SuscripcionId.Value,
                    Tipo = solicitud.BarberoId != null ? "Barbero" : "Barbería",
                    Email = solicitud.BarberoId != null ? solicitud.Barbero.Usuario.Correo : solicitud.BarberiaId != null ? solicitud.Barberia.Usuario.Correo : "-",
                    NombreUsuario = solicitud.BarberoId != null ? solicitud.Barbero.Usuario.NombreCompleto : solicitud.BarberiaId != null ? solicitud.Barberia.Usuario.NombreCompleto : "-",
                    NombreSuscripcionNuevo = suscripciones.FirstOrDefault(e => e.Id == solicitud.SuscripcionId)?.Nombre ?? "-",
                    NombreSuscripcionAnterior = suscripciones.FirstOrDefault(e => e.Id == suscripcionAnteriorId)?.Nombre ?? "-"
                };
                listaRetornar.Add(nuevaSoli);
            }
            return listaRetornar;
        }

        public async Task<DatosDashboardComercialDto> ObtenerDatosDashBoardComercial()
        {
            var totalBarberos = await TotalBarberos();
            var totalBarberias = await TotalBarberias();
            var totalSuscripcionesVencidas = await TotalSuscripcionesVencidas();
            var listaSolicitudesSuscripcion = await ObtenerListaSolicitudesSuscripcion();

            return new DatosDashboardComercialDto
            {
                TotalBarberos = totalBarberos,
                TotalBarberias = totalBarberias,
                TotalSuscripcionesVencidas = totalSuscripcionesVencidas,
                ListaSolicitudesSuscripcion = listaSolicitudesSuscripcion,
            };
        }

        public async Task<List<DatosBarberiasBarberosComercialDto>> ObtenerBarberiasYBarberos()
        {
            List<DatosBarberiasBarberosComercialDto> listaRetorno = new();

            var barberias = await _repositorios.Barberias
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Usuario)
                                    .Include(e => e.Suscripcion)
                                    .Include(e => e.SolicitudDeSuscripcion)
                                    .Select(e => new DatosBarberiasBarberosComercialDto
                                    {
                                        NombreUsuario = e.Usuario.NombreCompleto,
                                        NombreBarberia = e.Nombre ?? "-",
                                        Email = e.Usuario.Correo,
                                        SolicitudSuscripcionId = e.SolicitudDeSuscripcion.Id,
                                        EstadoSolicitud = e.EstadoSuscripcion.ToString() ?? EstadoSuscripcion.Pendiente.ToString(),
                                        Suscripcion = e.Suscripcion.Nombre ?? "-",
                                        Telefono = e.Telefono ?? "-",
                                        FechaVencimiento = e.FechaVencimientoSuscripcion != null ? e.FechaVencimientoSuscripcion.Value.ToString("dd/MM/yyyy") : "-",
                                        FechaSolicitud = e.SolicitudDeSuscripcion != null ? e.SolicitudDeSuscripcion.FechaSolicitado.Value.ToString("dd/MM/yyyy") : "-",
                                    })
                                    .ToListAsync();

            var barberos = await _repositorios.Barberos
                                   .GetQuery()
                                   .AsNoTracking()
                                   .Where(e => e.EstaAfiliadoABarberia.HasValue && !e.EstaAfiliadoABarberia.Value)
                                   .Include(e => e.Usuario)
                                   .Include(e => e.Suscripcion)
                                   .Include(e => e.SolicitudDeSuscripcion)
                                   .Select(e => new DatosBarberiasBarberosComercialDto
                                   {
                                       NombreUsuario = e.Usuario.NombreCompleto,
                                       NombreBarberia = e.Usuario.NombreCompleto ?? "-",
                                       Email = e.Usuario.Correo,
                                       SolicitudSuscripcionId = e.SolicitudDeSuscripcion.Id,
                                       EstadoSolicitud = e.EstadoSuscripcion.ToString() ?? EstadoSuscripcion.Pendiente.ToString(),
                                       Suscripcion = e.Suscripcion.Nombre ?? "-",
                                       Telefono = e.Telefono ?? "-",
                                       FechaVencimiento = e.FechaVencimientoSuscripcion != null ? e.FechaVencimientoSuscripcion.Value.ToString("dd/MM/yyyy") : "-",
                                       FechaSolicitud = e.SolicitudDeSuscripcion != null ? e.SolicitudDeSuscripcion.FechaSolicitado.Value.ToString("dd/MM/yyyy") : "-",
                                   })
                                   .ToListAsync();
            listaRetorno.AddRange(barberias);
            listaRetorno.AddRange(barberos);
            return listaRetorno;
        }

    }
}