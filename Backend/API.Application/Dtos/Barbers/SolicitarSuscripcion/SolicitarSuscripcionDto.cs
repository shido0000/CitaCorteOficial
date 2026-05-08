namespace API.Application.Dtos.Barbers.SolicitarSuscripcion
{
    public class SolicitarSuscripcionDto
    {
        public Guid NuevaSuscripcionId { get; set; }
        public Guid? BarberiaId { get; set; }
        public Guid? BarberoId { get; set; }
    }
}
