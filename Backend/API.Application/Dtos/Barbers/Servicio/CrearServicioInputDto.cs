using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Servicio
{
    public class CrearServicioInputDto : ServicioDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
