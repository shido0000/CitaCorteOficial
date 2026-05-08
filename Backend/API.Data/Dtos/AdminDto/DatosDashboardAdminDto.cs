using API.Data.Dtos.SuscripcionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.AdminDto
{
    public class DatosDashboardAdminDto
    {
        public required int TotalBarberos { get; set; }
        public required int TotalBarberias { get; set; }
        public required decimal TotalIngresosMes { get; set; }
        public required int TotalSuscripcionesVencidas { get; set; }
        public required List<SolicitudSuscripcionDto> ListaSolicitudesSuscripcion { get; set; } = new();
    }
}
