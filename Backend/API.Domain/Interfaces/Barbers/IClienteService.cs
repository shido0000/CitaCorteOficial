using API.Data.Dtos.ClienteDto;
using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface IClienteService : IBaseService<Cliente, ClienteValidator>
    {
        Task<DatosDashboardClienteDto> ObtenerDatosDashBoardCliente(Guid clienteId);
    }
}