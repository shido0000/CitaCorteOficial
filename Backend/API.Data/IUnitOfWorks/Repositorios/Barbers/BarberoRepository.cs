using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class BarberoRepository : BaseRepository<Barbero>, IBarberoRepository
    {
        public BarberoRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
