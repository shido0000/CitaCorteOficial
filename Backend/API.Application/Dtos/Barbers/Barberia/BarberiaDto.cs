using API.Application.Dtos.Comunes;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Barberia
{
    public class BarberiaDto : EntidadBaseDto
    {
        public Guid UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? LogoUrl { get; set; }
        public EstadoSuscripcion? EstadoSuscripcion { get; set; }
        public Guid? SuscripcionId { get; set; }
        public string? HorarioApertura { get; set; }
        public string? HorarioCierre { get; set; }
    }
}
