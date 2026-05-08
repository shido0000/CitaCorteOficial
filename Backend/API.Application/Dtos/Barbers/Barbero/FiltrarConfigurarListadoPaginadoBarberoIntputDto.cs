using API.Application.Dtos.Comunes;

namespace API.Application.Dtos.Barbers.Barbero
{
    public class FiltrarConfigurarListadoPaginadoBarberoIntputDto : ConfiguracionListadoPaginadoDto
    {
        public Guid? BarberiaId { get; set; }
    }
}
