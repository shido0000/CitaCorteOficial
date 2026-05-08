using API.Application.Dtos.Comunes;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Suscripcion
{
    public class SuscripcionDto : EntidadBaseDto
    {
        public TipoSuscripcion? TipoSuscripcion { get; set; }
        public NivelSuscripcion? NivelSuscripcion { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public bool? EsFree { get; set; }
        public int? TiempoVigencia { get; set; } // en dias
        public Guid? MonedaId { get; set; }
    }
}
