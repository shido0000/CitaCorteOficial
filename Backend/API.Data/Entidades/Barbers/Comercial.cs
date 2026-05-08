using API.Data.Entidades;
using API.Data.Entidades.Seguridad;

namespace API.Data.Entidades.Barbers
{
    public class Comercial : EntidadBase
    {
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
