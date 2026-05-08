using API.Data.Dtos.UsuarioDto;
using API.Data.Entidades.Seguridad;
using API.Domain.Validators.Seguridad;
using Microsoft.EntityFrameworkCore.Query;

namespace API.Domain.Interfaces.Seguridad
{
    public interface IUsuarioService : IBaseService<Usuario, UsuarioValidator>
    {
        Task CambiarContrasenna(Guid usuarioId, string contrasenna, bool debeCambiarContrasenna = false);
        Task<List<Permiso>> ObtenerPermisos(string username);
        Task<Usuario?> ObtenerPorUsername(string username, Func<IQueryable<Usuario>, IIncludableQueryable<Usuario, object>>? propiedadesIncluidas = null);
        Task<Guid> CrearUsusarioMejorado(UsuarioNuevoDto usuario);
        Task<UsuarioPerfilDto> ObtenerDatosUsuario(string username);
        Task<UsuarioPerfilDto> ObtenerDatosUsuarioPorId(Guid id);
        Task<Guid> ActualizarUsusarioMejorado(UsuarioActualizarDto user);
        Task<bool> CambiarEstadoUsuario(Guid id);
    }
}