using API.Data.Dtos.BarberoDto;
using API.Data.Dtos.ServicioDto;

namespace API.Data.Dtos.BarberiaDto
{
    public class BarberiaDatosDto
    {
        public required Guid Id { get; set; }
        public required string NombreBarberia { get; set; }
        public required decimal Clasificacion { get; set; }
        public required string Direccion { get; set; }
        public required string FotoUrl { get; set; }
        public required string HorarioApertura { get; set; }
        public required string HorarioCierre { get; set; }
        public required List<ServicioDatosDto> Servicios { get; set; } = new();
        public required List<BarberoDatosDto> Barberos { get; set; } = new();
    }
}
