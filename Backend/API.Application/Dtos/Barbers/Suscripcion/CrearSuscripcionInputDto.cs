using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Suscripcion
{
    public class CrearSuscripcionInputDto : SuscripcionDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
