using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Barbero
{
    public class CrearBarberoInputDto : BarberoDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
