using API.Data.Entidades.Barbers;

namespace API.Data.Dtos.ReservaDto
{
    public class ReservasHoyDto
    {
        public required Guid Id { get; set; }
        public required decimal PrecioServicio { get; set; }
        public required string EstadoDeReserva { get; set; }
        public required string NombreServicio { get; set; }
        public required string NombreCliente { get; set; }
        public required DateTime FechaReserva { get; set; }
        public required int DuracionEnMinutos { get; set; }
    }
}
