using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface ISolicitudDeAfiliacionService : IBaseService<SolicitudDeAfiliacion, SolicitudDeAfiliacionValidator>
    {
        Task<bool> SolicitarNuevaAfiliacion(Guid barberiaId, Guid barberoId);
        Task AprobarSolicitud(Guid solicitudId);
        Task<List<SolicitudDeAfiliacion>> ObtenerSolicitudesDeBarbero(Guid barberoId);
        Task<SolicitudDeAfiliacion?> ObtenerAfiliacionActivaDeBarbero(Guid barberoId);
    }
}