using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class SolicitudDeAfiliacion : EntidadBase
    {
        public Guid? BarberiaId { get; set; }
        public Barberia? Barberia { get; set; }
        public Guid? BarberoId { get; set; }
        public Barbero? Barbero { get; set; }
        public DateTime? FechaSolicitado { get; set; }
        public DateTime? FechaAprobado { get; set; }
        public EstadoSolicitudAfiliacion? EstadoSolicitudAfiliacion { get; set; }
    }
}
