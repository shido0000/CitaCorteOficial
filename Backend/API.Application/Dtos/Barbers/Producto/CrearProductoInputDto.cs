using System.Text.Json.Serialization;

namespace API.Application.Dtos.Barbers.Producto
{
    public class CrearProductoInputDto : ProductoDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
         
    }
}
