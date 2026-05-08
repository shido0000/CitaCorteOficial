using API.Application.Dtos.Comunes;
using API.Data.Enum;

namespace API.Application.Dtos.Barbers.Suscripcion
{
    public class FiltrarConfigurarListadoPaginadoSuscripcionIntputDto : ConfiguracionListadoPaginadoDto
    {
        public TipoSuscripcion? TipoSuscripcion { get; set; }
    }
}
