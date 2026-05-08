using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class SolicitudDeSuscripcionRepository : BaseRepository<SolicitudDeSuscripcion>, ISolicitudDeSuscripcionRepository
    {
        public SolicitudDeSuscripcionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
