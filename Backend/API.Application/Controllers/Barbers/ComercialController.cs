using API.Application.Dtos.Barbers.Comercial;
using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class ComercialController : BasicController<Comercial, ComercialValidator, DetallesComercialDto, CrearComercialInputDto, ActualizarComercialInputDto, ListadoPaginadoComercialDto, FiltrarConfigurarListadoPaginadoComercialIntputDto>
    {
        private ISolicitudDeSuscripcionService _solicitudDeSuscripcionService;
        private IComercialService _ComercialService;
        public ComercialController(IMapper mapper, IComercialService servicioComercial, ISolicitudDeSuscripcionService solicitudDeSuscripcionService) : base(mapper, servicioComercial)
        {
            _solicitudDeSuscripcionService = solicitudDeSuscripcionService;
            _ComercialService = servicioComercial;
        }

        protected override Task<(IEnumerable<Comercial>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoComercialIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Comercial, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Comercial => Comercial.Usuario.NombreCompleto.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Comercial.Usuario.Correo.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Comercial.Usuario.Username.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        /// <summary>
        /// Aprobar Solicitud de Suscripcion
        /// </summary>
        /// <param name="solicitudId">Id del elementos</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> AprobarSolicitud(Guid solicitudId)
        {
            await _solicitudDeSuscripcionService.AprobarSolicitud(solicitudId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK });
        }

        /// <summary>
        /// Obtener Datos DashBoard Comercial
        /// </summary>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerDatosDashBoardComercial()
        {
            var result = await _ComercialService.ObtenerDatosDashBoardComercial();
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Barberias Y Barberos
        /// </summary>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerBarberiasYBarberos()
        {
            var result = await _ComercialService.ObtenerBarberiasYBarberos();
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }
    }
}
