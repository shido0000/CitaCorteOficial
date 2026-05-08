using API.Data.Entidades;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class Servicio : EntidadBase
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string? FotoUrl { get; set; }
        public int? TiempoDemora { get; set; } // en minutos
        public Guid? BarberiaId { get; set; }
        public Barberia? Barberia { get; set; }
        public Guid? BarberoId { get; set; }
        public Barbero? Barbero { get; set; }
        public Guid? MonedaId { get; set; }
        public Moneda? Moneda { get; set; }
    }
}
