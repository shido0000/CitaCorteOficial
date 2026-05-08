using API.Data.Dtos.SuscripcionDto;
using API.Data.Entidades.Barbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.BarberiaDto
{
    public class BarberiaRecomendadaDto
    {
        public required Guid Id { get; set; }
        public required string NombreBarberia { get; set; }
        public required decimal Clasificacion { get; set; }
        public required string Direccion { get; set; }
        public required string FotoUrl { get; set; }
    }
}
