using API.Data.Dtos.ReservaDto;
using API.Data.Dtos.SuscripcionDto;

namespace API.Data.Dtos.BarberoDto
{
    public class DatosDashboardBarberoDto
    {
        public required Guid Id { get; set; }
        public required int CantidadReservasHoy { get; set; }
        public required int CantidadReservasCompletadasHoy { get; set; }
        public required decimal IngresosHoy { get; set; }
        public required decimal Calificacion { get; set; }
        public required SuscripcionActualBarberoDto SuscripcionActualDto { get; set; }
        public required List<ReservasHoyDto> ReservasParaHoy { get; set; } = new();
    }
}
