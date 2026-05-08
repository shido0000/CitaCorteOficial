using API.Data.Dtos.SuscripcionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.BarberiaDto
{
    public class DatosEstadisticaBarberiaDto
    {
        public required Guid Id { get; set; }
        public required int TotalReservas { get; set; }
        public required int ClientesUnicos { get; set; }
        public required decimal Ingresos { get; set; }
        public required decimal CalificacionPromedio { get; set; }
    }
}
