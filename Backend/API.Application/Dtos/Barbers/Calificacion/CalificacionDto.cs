using API.Application.Dtos.Comunes;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Calificacion
{
    public class CalificacionDto : EntidadBaseDto
    {
        public Guid UsuarioId { get; set; }
        public Guid? BarberiaId { get; set; }
        public Guid? BarberoId { get; set; }
        public string? Texto { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
