namespace API.Application.Dtos.Barbers.Suscripcion
{
    public class DetallesSuscripcionDto : SuscripcionDto
    {
        public List<string> CaracteristicaSuscripcion { get; set; } = new();
        public string CodigoMoneda { get; set; } = "-";
        public string DescripcionMoneda { get; set; } = "-";
    }
}
