namespace API.Application.Dtos.Barbers.SolicitarAfiliacion
{
    public class SolicitarAfiliacionDto
    {
        public required Guid BarberiaId { get; set; }
        public required Guid BarberoId { get; set; }
    }
}
