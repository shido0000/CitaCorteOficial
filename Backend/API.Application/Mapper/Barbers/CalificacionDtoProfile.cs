using API.Application.Dtos.Barbers.Calificacion;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class CalificacionDtoProfile : BaseProfile<Calificacion, CalificacionDto, CrearCalificacionInputDto, ActualizarCalificacionInputDto, ListadoPaginadoCalificacionDto>
    {
        public CalificacionDtoProfile()
        {
            MapCalificacionDto();
        }

        public void MapCalificacionDto()
        {
            CreateMap<Calificacion, DetallesCalificacionDto>()
                .ReverseMap();
        }
    }
}


