using API.Application.Dtos.Comunes;
using API.Data.Entidades.Seguridad;
using API.Data.Enum;
using API.Data.Enum.Nomencladores;

namespace API.Application.Dtos.Barbers.Cliente
{
    public class ClienteDto : EntidadBaseDto
    {
        public Guid UsuarioId { get; set; }
    }
}
