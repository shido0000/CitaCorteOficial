using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Barberia
{
    public class CrearBarberiaInputDto : BarberiaDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
