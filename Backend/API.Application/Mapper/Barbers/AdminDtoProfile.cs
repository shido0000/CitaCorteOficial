using API.Application.Dtos.Barbers.Admin;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class AdminDtoProfile : BaseProfile<Admin, AdminDto, CrearAdminInputDto, ActualizarAdminInputDto, ListadoPaginadoAdminDto>
    {
        public AdminDtoProfile()
        {
            MapAdminDto();
        }

        public void MapAdminDto()
        {
            CreateMap<Admin, DetallesAdminDto>()
                .ReverseMap();
        }
    }
}


