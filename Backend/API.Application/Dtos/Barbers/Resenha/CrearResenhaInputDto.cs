using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Resenha
{
    public class CrearResenhaInputDto : ResenhaDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
