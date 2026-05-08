using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Barbero
{
    public class BarberoDto : EntidadBaseDto
    {
        public Guid UsuarioId { get; set; }
        public Guid? BarberiaId { get; set; }
        public Guid? SuscripcionId { get; set; }
        public bool? EstaAfiliadoABarberia { get; set; }
        public EstadoSuscripcion? EstadoSuscripcion { get; set; }
        public string? HorarioApertura { get; set; }
        public string? HorarioCierre { get; set; }
    }
}
