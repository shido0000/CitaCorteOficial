using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface IReservaService : IBaseService<Reserva, ReservaValidator>
    {
        Task<Guid> ConfirmarReserva(Guid reservaId);
        Task<Guid> CancelarReserva(Guid reservaId, bool canceloCliente, Guid clienteTrabajadorId);
    }
}