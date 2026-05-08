using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;

namespace API.Domain.Services.Barbers
{
    public class MonedaService : BasicService<Moneda, MonedaValidator>, IMonedaService
    {

        public MonedaService(IUnitOfWork<Moneda> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

    }
}