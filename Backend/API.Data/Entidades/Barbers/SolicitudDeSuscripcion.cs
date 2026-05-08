using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class SolicitudDeSuscripcion : EntidadBase
    {
        public Guid? BarberiaId { get; set; }
        public Barberia? Barberia { get; set; }
        public Guid? BarberoId { get; set; }
        public Barbero? Barbero { get; set; }
        public Guid? SuscripcionId { get; set; }
        public Suscripcion? Suscripcion { get; set; }
        public DateTime? FechaSolicitado { get; set; }
        public DateTime? FechaAprobado { get; set; }
        public EstadoSuscripcion? EstadoSuscripcion { get; set; }
        public string? MotivoRechazo { get; set; }
    }
}
