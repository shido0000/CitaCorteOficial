using API.Application.Dtos.Barbers.Barbero;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class BarberoDtoProfile : BaseProfile<Barbero, BarberoDto, CrearBarberoInputDto, ActualizarBarberoInputDto, ListadoPaginadoBarberoDto>
    {
        public BarberoDtoProfile()
        {
            MapBarberoDto();
        }

        public void MapBarberoDto()
        {
            CreateMap<Barbero, DetallesBarberoDto>()
                .ReverseMap();
        }
    }
}


