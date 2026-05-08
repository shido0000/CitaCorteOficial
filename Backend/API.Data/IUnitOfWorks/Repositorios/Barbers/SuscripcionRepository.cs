using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class SuscripcionRepository : BaseRepository<Suscripcion>, ISuscripcionRepository
    {
        public SuscripcionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
