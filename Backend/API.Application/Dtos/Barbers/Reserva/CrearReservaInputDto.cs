using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Reserva
{
    public class CrearReservaInputDto : ReservaDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
