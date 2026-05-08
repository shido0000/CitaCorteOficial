using API.Application.Dtos.Barbers.Producto;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class ProductoDtoProfile : BaseProfile<Producto, ProductoDto, CrearProductoInputDto, ActualizarProductoInputDto, ListadoPaginadoProductoDto>
    {
        public ProductoDtoProfile()
        {
            MapProductoDto();
        }

        public void MapProductoDto()
        {
            CreateMap<Producto, DetallesProductoDto>()
                .ReverseMap();
        }
    }
}


