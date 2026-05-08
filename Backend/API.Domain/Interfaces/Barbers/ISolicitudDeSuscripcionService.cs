using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface ISolicitudDeSuscripcionService : IBaseService<SolicitudDeSuscripcion, SolicitudDeSuscripcionValidator>
    {
        Task<bool> SolicitarNuevaSuscripcion(Guid nuevaSuscripcionId, Guid? barberiaId, Guid? barberoId);
        Task AprobarSolicitud(Guid solicitudId);
        Task RechazarSolicitud(Guid solicitudId, string motivoRechazo);
    }
}