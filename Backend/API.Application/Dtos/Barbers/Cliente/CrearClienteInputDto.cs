using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Cliente
{
    public class CrearClienteInputDto : ClienteDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
