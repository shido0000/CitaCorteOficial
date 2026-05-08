using API.Data.Dtos.ServicioDto;

namespace API.Data.Dtos.BarberoDto
{
    public class BarberoDatosDto
    {
        public required Guid Id { get; set; }
        public required string Nombre { get; set; }
    }
}
