using API.Application.Dtos.Barbers.Resenha;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class ResenhaDtoProfile : BaseProfile<Resenha, ResenhaDto, CrearResenhaInputDto, ActualizarResenhaInputDto, ListadoPaginadoResenhaDto>
    {
        public ResenhaDtoProfile()
        {
            MapResenhaDto();
        }

        public void MapResenhaDto()
        {
            CreateMap<Resenha, DetallesResenhaDto>()
                .ReverseMap();
        }
    }
}


