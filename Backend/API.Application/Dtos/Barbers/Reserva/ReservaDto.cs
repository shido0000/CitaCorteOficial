using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Reserva
{
    public class ReservaDto : EntidadBaseDto
    {
        public Guid? ClienteId { get; set; }
        public Guid? BarberoId { get; set; }
        public Guid? BarberiaId { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Hora { get; set; }
        public string? Nota { get; set; }
        public Guid? ServicioId { get; set; }
        public EstadoReserva? EstadoReserva { get; set; }
    }
}
