using API.Application.Dtos.Comunes;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Moneda
{
    public class MonedaDto : EntidadBaseDto
    {
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public decimal? TasaEnMN { get; set; }
    }
}
