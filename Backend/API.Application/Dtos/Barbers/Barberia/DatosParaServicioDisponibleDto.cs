using API.Data.Enum;

namespace API.Application.Dtos.Barbers.Barberia
{
    public class DatosParaServicioDisponibleDto
    {
        public Guid ServicioId { get; set; }
        public Guid BarberiaId { get; set; }
        public Guid BarberoId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
