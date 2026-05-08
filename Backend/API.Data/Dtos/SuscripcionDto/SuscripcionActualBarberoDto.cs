using API.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.SuscripcionDto
{
    public class SuscripcionActualBarberoDto
    {
        public required string NivelSuscripcion { get; set; }
        public required string Nombre { get; set; }
        public required decimal Precio { get; set; }
        public required int TiempoVigencia { get; set; } // en dias
        public required DateTime FechaVencimiento { get; set; } 
        public required string Estado { get; set; } 
        public required List<string> Caracteristicas { get; set; } = new();
    }
}
