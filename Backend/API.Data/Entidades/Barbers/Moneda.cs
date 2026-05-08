using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class Moneda : EntidadBase
    {
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public decimal? TasaEnMN { get; set; }
        public List<Producto> Productos { get; set; } = new();
        public List<Servicio> Servicios { get; set; } = new();
        public List<Suscripcion> Suscripciones { get; set; } = new();

    }
}
