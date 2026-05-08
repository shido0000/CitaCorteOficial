namespace API.Data.Dtos.ServicioDto
{
    public class ServicioDatosDto
    {
        public required Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required decimal Precio { get; set; }
        public required int Duracion { get; set; }
    }
}
