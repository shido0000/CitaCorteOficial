using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;

namespace API.Domain.Services.Barbers
{
    public class NotificacionService : BasicService<Notificacion, NotificacionValidator>, INotificacionService
    {

        public NotificacionService(IUnitOfWork<Notificacion> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

    }
}