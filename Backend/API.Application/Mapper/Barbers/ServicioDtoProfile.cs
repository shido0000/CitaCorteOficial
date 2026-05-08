using API.Application.Dtos.Barbers.Servicio;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class ServicioDtoProfile : BaseProfile<Servicio, ServicioDto, CrearServicioInputDto, ActualizarServicioInputDto, ListadoPaginadoServicioDto>
    {
        public ServicioDtoProfile()
        {
            MapServicioDto();
        }

        public void MapServicioDto()
        {
            CreateMap<Servicio, DetallesServicioDto>()
                .ReverseMap();
        }
    }
}


