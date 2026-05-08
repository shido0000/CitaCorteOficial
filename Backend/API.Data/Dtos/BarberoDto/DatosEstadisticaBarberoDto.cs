namespace API.Data.Dtos.BarberoDto
{
    public class DatosEstadisticaBarberoDto
    {
        public required Guid Id { get; set; }
        public required int TotalReservas { get; set; }
        public required int ClientesUnicos { get; set; }
        public required int TotalServiciosEnMes { get; set; }
        public required decimal Ingresos { get; set; }
        public required decimal CalificacionPromedio { get; set; }
        public required List<ServiciosMasSolicitadosBarberoDto> ServiciosMasSolicitadosBarberoDto { get; set; } = new();
    }
}
