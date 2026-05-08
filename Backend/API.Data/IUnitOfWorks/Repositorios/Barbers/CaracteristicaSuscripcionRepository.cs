using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class CaracteristicaSuscripcionRepository : BaseRepository<CaracteristicaSuscripcion>, ICaracteristicaSuscripcionRepository
    {
        public CaracteristicaSuscripcionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
