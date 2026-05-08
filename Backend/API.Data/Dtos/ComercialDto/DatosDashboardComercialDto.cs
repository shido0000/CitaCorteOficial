using API.Data.Dtos.SuscripcionDto;

namespace API.Data.Dtos.ComercialDto
{
    public class DatosDashboardComercialDto
    {
        public required int TotalBarberos { get; set; }
        public required int TotalBarberias { get; set; }
        public required int TotalSuscripcionesVencidas { get; set; }
        public required List<SolicitudSuscripcionDto> ListaSolicitudesSuscripcion { get; set; } = new();
    }
}
