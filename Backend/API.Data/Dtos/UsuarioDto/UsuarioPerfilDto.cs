using API.Data.Entidades.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.UsuarioDto
{
    public class UsuarioPerfilDto
    {
        public required Guid Id { get; set; }
        public required Guid RolId { get; set; }
        public required string NombreRol { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string Username { get; set; }
        public required string Correo { get; set; }

        public string? NombreBarberia { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? LogoUrl { get; set; }
        public Guid? SuscripcionId { get; set; }

    }
}
