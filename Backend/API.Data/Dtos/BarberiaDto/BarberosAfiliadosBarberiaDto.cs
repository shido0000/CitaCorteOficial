using API.Data.Dtos.SuscripcionDto;
using API.Data.Entidades.Barbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.BarberiaDto
{
    public class BarberosAfiliadosBarberiaDto
    {
        public required Guid Id { get; set; }
        public required int CuposDisponibles { get; set; }
        public required int CantidadBarberosAfiliados { get; set; }
        public required List<Barbero> Barberos { get; set; }
        public required List<SolicitudDeAfiliacion> SolicitudesPendientes { get; set; }
    }
}
