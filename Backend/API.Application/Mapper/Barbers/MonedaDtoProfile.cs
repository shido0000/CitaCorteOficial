using API.Application.Dtos.Barbers.Moneda;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class MonedaDtoProfile : BaseProfile<Moneda, MonedaDto, CrearMonedaInputDto, ActualizarMonedaInputDto, ListadoPaginadoMonedaDto>
    {
        public MonedaDtoProfile()
        {
            MapMonedaDto();
        }

        public void MapMonedaDto()
        {
            CreateMap<Moneda, DetallesMonedaDto>()
                .ReverseMap();
        }
    }
}


