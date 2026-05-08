using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Calificacion
{
    public class CrearCalificacionInputDto : CalificacionDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
