using API.Application.Dtos.Barbers.Comercial;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class ComercialDtoProfile : BaseProfile<Comercial, ComercialDto, CrearComercialInputDto, ActualizarComercialInputDto, ListadoPaginadoComercialDto>
    {
        public ComercialDtoProfile()
        {
            MapComercialDto();
        }

        public void MapComercialDto()
        {
            CreateMap<Comercial, DetallesComercialDto>()
                .ReverseMap();
        }
    }
}


