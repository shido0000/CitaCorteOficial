using API.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.SuscripcionDto
{
    public class SolicitudSuscripcionDto
    {
        public required string Nombre { get; set; }
        public required string NombreSuscripcionAnterior { get; set; }
        public required string NombreSuscripcionNuevo { get; set; }
        public required string NombreUsuario { get; set; }
        public required Guid SuscripcionNuevaId { get; set; }
        public required string Estado { get; set; } 
        public required string FechaSolicitud { get; set; } 
        public required string Tipo { get; set; } 
        public required string Email { get; set; } 
    }
}
