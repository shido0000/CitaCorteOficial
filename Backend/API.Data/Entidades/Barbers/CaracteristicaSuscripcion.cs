using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;

namespace API.Data.Entidades.Barbers
{
    public class CaracteristicaSuscripcion : EntidadBase
    {
        public string? Descripcion { get; set; }
        public Guid? SuscripcionId { get; set; }
        public Suscripcion? Suscripcion { get; set; }
    }
}
