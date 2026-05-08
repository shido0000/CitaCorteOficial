namespace API.Application.Dtos.Barbers.SolicitarSuscripcion
{
    public class RechazarSuscripcionDto
    {
        public required Guid SolicitudId { get; set; }
        public required string MotivoRechazo { get; set; }
    }
}
