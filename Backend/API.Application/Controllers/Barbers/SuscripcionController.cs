using API.Application.Dtos.Barbers.Suscripcion;
using API.Data.Entidades.Barbers;
using API.Data.Enum;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class SuscripcionController : BasicController<Suscripcion, SuscripcionValidator, DetallesSuscripcionDto, CrearSuscripcionInputDto, ActualizarSuscripcionInputDto, ListadoPaginadoSuscripcionDto, FiltrarConfigurarListadoPaginadoSuscripcionIntputDto>
    {

        public SuscripcionController(IMapper mapper, ISuscripcionService servicioSuscripcion) : base(mapper, servicioSuscripcion)
        {
        }

        protected override Task<(IEnumerable<Suscripcion>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoSuscripcionIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Suscripcion, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Suscripcion => Suscripcion.Nombre.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Suscripcion.Descripcion.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            if (inputDto.TipoSuscripcion.HasValue)
            {
                if (inputDto.TipoSuscripcion == TipoSuscripcion.Barberia)
                {
                    filtros.Add(Suscripcion => Suscripcion.TipoSuscripcion == TipoSuscripcion.Barberia);
                }
                if (inputDto.TipoSuscripcion == TipoSuscripcion.Barbero)
                {
                    filtros.Add(Suscripcion => Suscripcion.TipoSuscripcion == TipoSuscripcion.Barbero);
                }
            }
            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, query=>query.Include(e=>e.CaracteristicaSuscripciones), filtros.ToArray());
        }

    }
}
