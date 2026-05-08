using API.Data.Dtos.SuscripcionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.ComercialDto
{
    public class DatosBarberiasBarberosComercialDto
    {
        public required string NombreUsuario { get; set; }
        public required string NombreBarberia { get; set; }
        public required string Suscripcion { get; set; }
        public required string FechaSolicitud { get; set; }
        public required string FechaVencimiento { get; set; }
        public required string EstadoSolicitud { get; set; }
        public required string Email { get; set; }
        public required string Telefono { get; set; }
        public required Guid SolicitudSuscripcionId { get; set; }
    }
}
