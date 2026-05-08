using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class Suscripcion : EntidadBase
    {
        public TipoSuscripcion? TipoSuscripcion { get; set; }
        public NivelSuscripcion? NivelSuscripcion { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public bool? EsFree { get; set; }
        public int? TiempoVigencia { get; set; } // en dias
        public Guid? MonedaId { get; set; }
        public Moneda? Moneda { get; set; }
        public List<Barbero> Barberos { get; set; } = new();
        public List<Barberia> Barberias { get; set; } = new();
        public List<CaracteristicaSuscripcion> CaracteristicaSuscripciones { get; set; } = new();
        public List<SolicitudDeSuscripcion> SolicitudDeSuscripciones { get; set; } = new();
    }
}
