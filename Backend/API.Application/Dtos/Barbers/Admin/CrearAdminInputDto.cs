using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Admin
{
    public class CrearAdminInputDto : AdminDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
