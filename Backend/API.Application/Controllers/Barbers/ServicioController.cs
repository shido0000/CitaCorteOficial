using API.Application.Dtos.Barbers.Servicio;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class ServicioController : BasicController<Servicio, ServicioValidator, DetallesServicioDto, CrearServicioInputDto, ActualizarServicioInputDto, ListadoPaginadoServicioDto, FiltrarConfigurarListadoPaginadoServicioIntputDto>
    {

        public ServicioController(IMapper mapper, IServicioService servicioServicio) : base(mapper, servicioServicio)
        {
        }

        protected override Task<(IEnumerable<Servicio>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoServicioIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Servicio, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Servicio => Servicio.Nombre.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Servicio.Descripcion.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            if (inputDto.BarberiaId.HasValue) {
                filtros.Add(Servicio => Servicio.BarberiaId==inputDto.BarberiaId.Value);
            }
            if (inputDto.BarberoId.HasValue)
            {
                filtros.Add(Servicio => Servicio.BarberoId == inputDto.BarberoId.Value);
            }
            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

    }
}
