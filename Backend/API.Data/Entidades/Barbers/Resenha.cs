using API.Data.Entidades.Seguridad;

namespace API.Data.Entidades.Barbers
{
    public class Resenha : EntidadBase
    {
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public Guid? BarberiaId { get; set; }
        public Barberia? Barberia { get; set; }
        public Guid? BarberoId { get; set; }
        public Barbero? Barbero { get; set; }
        public string? Texto { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
