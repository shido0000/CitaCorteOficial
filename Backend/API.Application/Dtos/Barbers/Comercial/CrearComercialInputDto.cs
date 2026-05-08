using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Comercial
{
    public class CrearComercialInputDto : ComercialDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
