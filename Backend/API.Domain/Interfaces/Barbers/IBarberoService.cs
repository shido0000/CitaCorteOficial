using API.Data.Dtos.BarberiaDto;
using API.Data.Dtos.BarberoDto;
using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface IBarberoService : IBaseService<Barbero, BarberoValidator>
    {
        Task<BarberosAfiliadosBarberiaDto> ObtenerBarberosAfiliadosABarberia(Guid barberiaId);
        Task<DatosEstadisticaBarberoDto> ObtenerEstadisticasBarbero(Guid barberoId);
        Task<DatosDashboardBarberoDto> ObtenerDatosDashBoardBarbero(Guid barberoId);
    }
}