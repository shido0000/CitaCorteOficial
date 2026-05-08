using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class ResenhaRepository : BaseRepository<Resenha>, IResenhaRepository
    {
        public ResenhaRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
