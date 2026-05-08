using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Domain.Services.Barbers
{
    public class SuscripcionService : BasicService<Suscripcion, SuscripcionValidator>, ISuscripcionService
    {

        public SuscripcionService(IUnitOfWork<Suscripcion> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public override async Task<(IEnumerable<Suscripcion>, int)> ObtenerListadoPaginado(int cantidadIgnorar, int? cantidadMostrar, string? secuenciaOrdenamiento, Func<IQueryable<Suscripcion>, IIncludableQueryable<Suscripcion, object>>? propiedadesIncluidas = null, params Expression<Func<Suscripcion, bool>>[] filtros)
        {
            if (cantidadIgnorar < 0)
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "La cantidad de elementos a ignorar debe ser mayor o igual a 0." };
            if (cantidadMostrar.HasValue && cantidadMostrar.Value <= 0)
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "La cantidad de elementos a mostrar debe ser mayor a 0." };

            IQueryable<Suscripcion> query = _repositorios.BasicRepository.GetQuery(propiedadesIncluidas);

            //Filtrando
            query = filtros.Aggregate(query, (current, filters) => current.Where(filters));
            //Ordenando
            query = OrdenarLista(query, secuenciaOrdenamiento);
            //Counting
            int total = await query.CountAsync();
            //Paginando
            var resultado = await query
               .Skip(cantidadIgnorar)
               .Take(cantidadMostrar.GetValueOrDefault(total))
               .Select(e => new Suscripcion
               {
                   Id = e.Id,
                   TipoSuscripcion = e.TipoSuscripcion,
                   NivelSuscripcion = e.NivelSuscripcion,
                   Nombre = e.Nombre,
                   Descripcion = e.Descripcion,
                   Precio = e.Precio,
                   EsFree = e.EsFree,
                   TiempoVigencia = e.TiempoVigencia,
                   MonedaId = e.MonedaId,
                   CaracteristicaSuscripciones = e.CaracteristicaSuscripciones.Select(e => new CaracteristicaSuscripcion
                   {
                       Id = e.Id,
                       Descripcion = e.Descripcion,
                   }).ToList(),
                   Moneda = e.Moneda == null ? null : new Moneda
                   {
                       Id = e.Moneda.Id,
                       Descripcion = e.Moneda.Descripcion,
                       Codigo = e.Moneda.Codigo,
                   },
               })
               .AsNoTracking()
               .ToListAsync();

            return (resultado, total);
        }
    }
}