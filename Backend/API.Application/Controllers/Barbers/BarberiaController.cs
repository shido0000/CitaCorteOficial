using API.Application.Dtos.Barbers.Barberia;
using API.Application.Dtos.Barbers.SolicitarSuscripcion;
using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class BarberiaController : BasicController<Barberia, BarberiaValidator, DetallesBarberiaDto, CrearBarberiaInputDto, ActualizarBarberiaInputDto, ListadoPaginadoBarberiaDto, FiltrarConfigurarListadoPaginadoBarberiaIntputDto>
    {
        private ISolicitudDeSuscripcionService _solicitudSuscripcionService;
        private IBarberiaService _barberiaServicio;
        private IBarberoService _barberoServicio;
        public BarberiaController(IMapper mapper, IBarberiaService servicioBarberia, ISolicitudDeSuscripcionService solicitudSuscripcionService, IBarberoService barberoServicio) : base(mapper, servicioBarberia)
        {
            _solicitudSuscripcionService = solicitudSuscripcionService;
            _barberiaServicio = servicioBarberia;
            _barberoServicio = barberoServicio;
        }

        protected override Task<(IEnumerable<Barberia>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoBarberiaIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Barberia, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Barberia => Barberia.Nombre.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Barberia.Direccion.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }


        /// <summary>
        /// Solicitar Nueva Suscripcion
        /// </summary>
        /// <param name="solicitarSuscripcionDto">Elemento a crear</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> SolicitarNuevaSuscripcion(SolicitarSuscripcionDto solicitarSuscripcionDto)
        {
            var result = await _solicitudSuscripcionService.SolicitarNuevaSuscripcion(solicitarSuscripcionDto.NuevaSuscripcionId, solicitarSuscripcionDto.BarberiaId, solicitarSuscripcionDto.BarberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Datos DashBoard Barberia
        /// </summary>
        /// <param name="barberiaId">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerDatosDashBoardBarberia(Guid barberiaId)
        {
            var result = await _barberiaServicio.ObtenerDatosDashBoardBarberia(barberiaId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }


        /// <summary>
        /// Obtener Barberos Afiliados A Barberia
        /// </summary>
        /// <param name="barberiaId">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerBarberosAfiliadosABarberia(Guid barberiaId)
        {
            var result = await _barberoServicio.ObtenerBarberosAfiliadosABarberia(barberiaId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Estadisticas Barberia
        /// </summary>
        /// <param name="barberiaId">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerEstadisticasBarberia(Guid barberiaId)
        {
            var result = await _barberiaServicio.ObtenerEstadisticasBarberia(barberiaId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Barberia por id
        /// </summary>
        /// <param name="id">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerDatos(Guid id)
        {
            var result = await _barberiaServicio.ObtenerDatosBarberiaPorId(id);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Horarios Disponibles Detallados
        /// </summary>
        /// <param name="datosParaServicioDisponibleDto">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> ObtenerHorariosDisponiblesDetallados(DatosParaServicioDisponibleDto datosParaServicioDisponibleDto)
        {
            var result = await _barberiaServicio.ObtenerHorariosDisponiblesDetallados(datosParaServicioDisponibleDto.ServicioId, datosParaServicioDisponibleDto.Fecha, datosParaServicioDisponibleDto.BarberiaId, datosParaServicioDisponibleDto.BarberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

    }
}
