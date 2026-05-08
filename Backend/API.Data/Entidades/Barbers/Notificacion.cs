using API.Data.Entidades;
using API.Data.Entidades.Seguridad;

namespace API.Data.Entidades.Barbers
{
    public class Notificacion : EntidadBase
    {
        public Guid? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string? Titulo { get; set; }
        public string? Mensaje { get; set; }
        public bool FueLeido { get; set; }
    }
}
