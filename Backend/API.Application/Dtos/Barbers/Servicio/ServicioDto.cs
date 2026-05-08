using API.Application.Dtos.Comunes;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Servicio
{
    public class ServicioDto : EntidadBaseDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string? FotoUrl { get; set; }
        public int? TiempoDemora { get; set; } // en minutos
        public Guid? BarberiaId { get; set; }
        public Guid? BarberoId { get; set; }
    }
}
