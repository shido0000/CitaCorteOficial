using API.Application.Dtos.Barbers.Reserva;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class ReservaDtoProfile : BaseProfile<Reserva, ReservaDto, CrearReservaInputDto, ActualizarReservaInputDto, ListadoPaginadoReservaDto>
    {
        public ReservaDtoProfile()
        {
            MapReservaDto();
        }

        public void MapReservaDto()
        {
            CreateMap<Reserva, DetallesReservaDto>()
                .ReverseMap();
        }
    }
}


