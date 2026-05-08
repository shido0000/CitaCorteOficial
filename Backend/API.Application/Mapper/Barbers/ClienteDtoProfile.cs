using API.Application.Dtos.Barbers.Cliente;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class ClienteDtoProfile : BaseProfile<Cliente, ClienteDto, CrearClienteInputDto, ActualizarClienteInputDto, ListadoPaginadoClienteDto>
    {
        public ClienteDtoProfile()
        {
            MapClienteDto();
        }

        public void MapClienteDto()
        {
            CreateMap<Cliente, DetallesClienteDto>()
                .ReverseMap();
        }
    }
}


