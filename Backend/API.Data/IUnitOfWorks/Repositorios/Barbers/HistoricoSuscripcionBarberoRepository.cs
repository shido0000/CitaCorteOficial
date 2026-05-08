using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class HistoricoSuscripcionBarberoRepository : BaseRepository<HistoricoSuscripcionBarbero>, IHistoricoSuscripcionBarberoRepository
    {
        public HistoricoSuscripcionBarberoRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
