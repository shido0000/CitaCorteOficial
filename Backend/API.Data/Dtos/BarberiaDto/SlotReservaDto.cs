using API.Data.Dtos.ServicioDto;

namespace API.Data.Dtos.BarberiaDto
{
    public class SlotReservaDto
    {
        public string Hora { get; set; } = string.Empty;
        public int DuracionMinutos { get; set; }
        public bool Disponible { get; set; }
        public Guid ServicioId { get; set; }
        public Guid BarberiaId { get; set; }
        public Guid BarberoId { get; set; }
    }

}
