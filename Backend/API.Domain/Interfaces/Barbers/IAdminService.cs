using API.Data.Dtos.AdminDto;
using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface IAdminService : IBaseService<Admin, AdminValidator>
    {
        Task<DatosDashboardAdminDto> ObtenerDatosDashBoardAdmin();
        Task<List<DatosBarberiasBarberosAdminDto>> ObtenerBarberiasYBarberos();
    }
}