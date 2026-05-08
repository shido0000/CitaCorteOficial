using API.Data.Dtos.SuscripcionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.BarberiaDto
{
    public class DatosDashboardBarberiaDto
    {
        public required Guid Id { get; set; }
        public required int CantidadReservasHoy { get; set; }
        public required int CantidadBarberosAfiliados { get; set; }
        public required decimal IngresosHoy { get; set; }
        public required decimal Calificacion { get; set; }
        public required SuscripcionActualDto SuscripcionActualDto { get; set; }
    }
}
