using API.Application.Dtos.Barbers.Producto;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class ProductoController : BasicController<Producto, ProductoValidator, DetallesProductoDto, CrearProductoInputDto, ActualizarProductoInputDto, ListadoPaginadoProductoDto, FiltrarConfigurarListadoPaginadoProductoIntputDto>
    {

        public ProductoController(IMapper mapper, IProductoService servicioProducto) : base(mapper, servicioProducto)
        {
        }

        protected override Task<(IEnumerable<Producto>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoProductoIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Producto, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Producto => Producto.Nombre.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Producto.Descripcion.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

    }
}
