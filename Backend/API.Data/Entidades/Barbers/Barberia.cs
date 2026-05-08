using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class Barberia : EntidadBase
    {
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? LogoUrl { get; set; }
        public string? HorarioApertura { get; set; }
        public string? HorarioCierre { get; set; }
        public EstadoSuscripcion? EstadoSuscripcion { get; set; }
        public Guid? SuscripcionId { get; set; }
        public Suscripcion? Suscripcion { get; set; }
        public DateTime? FechaVencimientoSuscripcion { get; set; }
        public List<Barbero> Barberos { get; set; } = new();
        public List<Producto> Productos { get; set; } = new();
        public List<Servicio> Servicios { get; set; } = new();
        public List<Reserva> Reservas { get; set; } = new();
        public SolicitudDeSuscripcion? SolicitudDeSuscripcion { get; set; }
        public SolicitudDeAfiliacion? SolicitudDeAfiliacion { get; set; }
        public List<Calificacion> Calificaciones { get; set; } = new();
        public List<Resenha> Resenhas { get; set; } = new();

    }
}
