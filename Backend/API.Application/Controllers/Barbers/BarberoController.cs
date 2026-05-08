using API.Application.Dtos.Barbers.Barbero;
using API.Application.Dtos.Comunes;
using API.Data.Dtos.BarberoDto;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class BarberoController : BasicController<Barbero, BarberoValidator, DetallesBarberoDto, CrearBarberoInputDto, ActualizarBarberoInputDto, ListadoPaginadoBarberoDto, FiltrarConfigurarListadoPaginadoBarberoIntputDto>
    {
        private readonly IBarberoService _barberoService;
        public BarberoController(IMapper mapper, IBarberoService servicioBarbero) : base(mapper, servicioBarbero)
        {
            _barberoService = servicioBarbero;
        }

        protected override Task<(IEnumerable<Barbero>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoBarberoIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Barbero, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Barbero => Barbero.Usuario.NombreCompleto.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Barbero.Usuario.Correo.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Barbero.Usuario.Username.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }
            if (inputDto.BarberiaId.HasValue)
            {
                filtros.Add(Servicio => Servicio.BarberiaId == inputDto.BarberiaId.Value);
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }


        /// <summary>
        /// Obtener Estadisticas Barbero
        /// </summary>
        /// <param name="barberoId">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerEstadisticasBarbero(Guid barberoId)
        {
            var result = await _barberoService.ObtenerEstadisticasBarbero(barberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Datos DashBoard Barbero
        /// </summary>
        /// <param name="barberoId">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerDatosDashBoardBarbero(Guid barberoId)
        {
            var result = await _barberoService.ObtenerDatosDashBoardBarbero(barberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }
    }
}
