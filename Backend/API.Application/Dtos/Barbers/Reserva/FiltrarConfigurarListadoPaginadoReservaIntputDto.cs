using API.Application.Dtos.Comunes;

namespace API.Application.Dtos.Barbers.Reserva
{
    public class FiltrarConfigurarListadoPaginadoReservaIntputDto : ConfiguracionListadoPaginadoDto
    {
        public Guid? BarberiaId { get; set; }
        public Guid? BarberoId { get; set; }
        public Guid? ClienteId { get; set; }
    }
}
