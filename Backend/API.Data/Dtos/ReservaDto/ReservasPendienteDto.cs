using API.Data.Entidades.Barbers;

namespace API.Data.Dtos.ReservaDto
{
    public class ReservasPendienteDto
    {
        public required Guid Id { get; set; }
        public required DateTime Fecha { get; set; }
        public required string Hora { get; set; }
        public required string NombreServicio { get; set; }
        public required string NombreBarberiaBarbero { get; set; }
        public required decimal Rating { get; set; }
        public required int Resenhas { get; set; }
        public required string Distancia { get; set; }
    }
}
