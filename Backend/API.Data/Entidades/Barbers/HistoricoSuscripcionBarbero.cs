using API.Data.Entidades;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class HistoricoSuscripcionBarbero : EntidadBase
    {
        public Guid? BarberoId { get; set; }
        public Barbero? Barbero { get; set; }
        public TipoSuscripcion? TipoSuscripcion { get; set; }
        public NivelSuscripcion? NivelSuscripcion { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public bool? EsFree { get; set; }
        public int? TiempoVigencia { get; set; } // en dias
    }
}
