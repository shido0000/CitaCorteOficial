using API.Data.Dtos.BarberiaDto;
using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface IBarberiaService : IBaseService<Barberia, BarberiaValidator>
    {
        Task<DatosDashboardBarberiaDto> ObtenerDatosDashBoardBarberia(Guid barberiaId);
        Task<DatosEstadisticaBarberiaDto> ObtenerEstadisticasBarberia(Guid barberiaId);
        Task<List<BarberiaRecomendadaDto>> ObtenerBarberiasRecomendadas();
        Task<BarberiaDatosDto> ObtenerDatosBarberiaPorId(Guid barberiaId);
        Task<List<SlotReservaDto>> ObtenerHorariosDisponiblesDetallados(Guid servicioId, DateTime fecha, Guid barberiaId, Guid barberoId);
    }
}