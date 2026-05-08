using API.Data.Entidades.Barbers;

namespace API.Data.Entidades.Seguridad
{
    /// <summary>
    /// Tabla que guarda datos de los usuarios del sistema
    /// </summary>
    public class Usuario : EntidadBase
    {
        public required Guid RolId { get; set; }

        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public string NombreCompleto { get => $"{Nombre} {Apellidos}"; }
        public required string Username { get; set; }
        public required string Contrasenna { get; set; }
        public required string Correo { get; set; }
        public bool DebeCambiarContrasenna { get; set; }
        public bool Activo { get; set; }

        public Rol Rol { get; set; } = null!;

        // Relaciones
        public Barbero? Barbero { get; set; }
        public Barberia? Barberia { get; set; }
        public Comercial? Comercial { get; set; }
        public Cliente? Cliente { get; set; }
        public List<Resenha> Resenhas { get; set; } = new();
        public List<Calificacion> Calificaciones { get; set; } = new();
    }
}