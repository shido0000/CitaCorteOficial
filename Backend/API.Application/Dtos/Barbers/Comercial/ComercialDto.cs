using API.Application.Dtos.Comunes;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Comercial
{
    public class ComercialDto : EntidadBaseDto
    {
        public Guid UsuarioId { get; set; }
    }
}
