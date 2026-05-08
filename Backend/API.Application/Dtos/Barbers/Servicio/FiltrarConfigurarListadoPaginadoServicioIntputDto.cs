using API.Application.Dtos.Comunes;

namespace API.Application.Dtos.Barbers.Servicio
{
    public class FiltrarConfigurarListadoPaginadoServicioIntputDto : ConfiguracionListadoPaginadoDto
    {
        public Guid? BarberiaId { get; set; }
        public Guid? BarberoId { get; set; }

    }
}
