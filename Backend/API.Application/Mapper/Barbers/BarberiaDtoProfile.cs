using API.Application.Dtos.Barbers.Barberia;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class BarberiaDtoProfile : BaseProfile<Barberia, BarberiaDto, CrearBarberiaInputDto, ActualizarBarberiaInputDto, ListadoPaginadoBarberiaDto>
    {
        public BarberiaDtoProfile()
        {
            MapBarberiaDto();
        }

        public void MapBarberiaDto()
        {
            CreateMap<Barberia, DetallesBarberiaDto>()
                .ReverseMap();
        }
    }
}


