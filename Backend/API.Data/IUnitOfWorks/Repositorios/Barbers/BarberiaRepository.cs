using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class BarberiaRepository : BaseRepository<Barberia>, IBarberiaRepository
    {
        public BarberiaRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
