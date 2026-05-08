using API.Data.Dtos.ComercialDto;
using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface IComercialService : IBaseService<Comercial, ComercialValidator>
    {
        Task<DatosDashboardComercialDto> ObtenerDatosDashBoardComercial();
        Task<List<DatosBarberiasBarberosComercialDto>> ObtenerBarberiasYBarberos();
    }
}