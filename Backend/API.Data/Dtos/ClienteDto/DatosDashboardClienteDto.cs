using API.Data.Dtos.BarberiaDto;
using API.Data.Dtos.ReservaDto;
using API.Data.Dtos.SuscripcionDto;

namespace API.Data.Dtos.ClienteDto
{
    public class DatosDashboardClienteDto
    {
        public required Guid Id { get; set; }
        public required int TotalReservas { get; set; }
        public required int TotalReservasCompletadas { get; set; }
        public required int TotalReservasProximas { get; set; }
        public required List<ReservasPendienteDto> ListadoReservasProximas { get; set; } = new();
        public required List<BarberiaRecomendadaDto> ListadBarberiasRecomendadas { get; set; } = new();
    }
}
