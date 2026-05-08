using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class CalificacionRepository : BaseRepository<Calificacion>, ICalificacionRepository
    {
        public CalificacionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
