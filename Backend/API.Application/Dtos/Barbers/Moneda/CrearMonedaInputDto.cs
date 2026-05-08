using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Moneda
{
    public class CrearMonedaInputDto : MonedaDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
