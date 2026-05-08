using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class ServicioRepository : BaseRepository<Servicio>, IServicioRepository
    {
        public ServicioRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
