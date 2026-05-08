using API.Data.Entidades;
using API.Data.Entidades.Seguridad;

namespace API.Data.Entidades.Barbers
{
    public class Cliente : EntidadBase
    {
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public List<Reserva> Reservas { get; set; } = new();
    }
}
