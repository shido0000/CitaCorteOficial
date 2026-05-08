using API.Application.Dtos.Barbers.Moneda;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class MonedaController : BasicController<Moneda, MonedaValidator, DetallesMonedaDto, CrearMonedaInputDto, ActualizarMonedaInputDto, ListadoPaginadoMonedaDto, FiltrarConfigurarListadoPaginadoMonedaIntputDto>
    {

        public MonedaController(IMapper mapper, IMonedaService servicioMoneda) : base(mapper, servicioMoneda)
        {
        }

        protected override Task<(IEnumerable<Moneda>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoMonedaIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Moneda, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Moneda => Moneda.Codigo.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Moneda.Descripcion.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

    }
}
