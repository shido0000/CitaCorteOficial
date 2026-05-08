using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class Reserva : EntidadBase
    {
        public Guid? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public Guid? BarberoId { get; set; }
        public Barbero? Barbero { get; set; }
        public Guid? BarberiaId { get; set; }
        public Barberia? Barberia { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Hora { get; set; }
        public string? Nota { get; set; }
        public Guid? ServicioId { get; set; }
        public Servicio? Servicio { get; set; }
        public EstadoReserva? EstadoReserva { get; set; } 
    }
}
