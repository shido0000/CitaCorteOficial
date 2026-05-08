using API.Data.DbContexts;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Barbers;

namespace API.Data.IUnitOfWorks.Repositorios.Barbers
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        public NotificacionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
