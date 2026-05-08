using API.Data.Dtos.SuscripcionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Dtos.BarberoDto
{
    public class ServiciosMasSolicitadosBarberoDto
    {
        public required string Nombre { get; set; }
        public required int Cantidad { get; set; }
    }
}
