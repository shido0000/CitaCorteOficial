using API.Data.Dtos.HomeDto;

namespace API.Domain.Interfaces.Barbers
{
    public interface IHomeService
    {
        Task<List<DatosBarberiasBarberosHomeDto>> ObtenerBarberiasYBarberosHome(string? texto);
    }
}