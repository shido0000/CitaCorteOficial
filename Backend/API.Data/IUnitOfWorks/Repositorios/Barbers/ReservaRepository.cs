using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
