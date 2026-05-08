using API.Application.Dtos.Barbers.SolicitarAfiliacion;
using API.Application.Dtos.Comunes;
using API.Application.Filters;
using API.Domain.Interfaces.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers.Seguridad
{

    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SolicitudAfiliacionController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ISolicitudDeAfiliacionService _SolicitudAfiliacionServicio;

        public SolicitudAfiliacionController(IMapper mapper, ISolicitudDeAfiliacionService SolicitudAfiliacionServicio)
        {
            _SolicitudAfiliacionServicio = SolicitudAfiliacionServicio;
            _mapper = mapper;
        }

        /// <summary>
        /// Solicitar Nueva Afiliación
        /// </summary>
        /// <param name="solicitarAfiliacionDto">Elemento a crear</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> SolicitarNuevaAfiliacion([FromBody] SolicitarAfiliacionDto solicitarAfiliacionDto)
        {
            var result = await _SolicitudAfiliacionServicio.SolicitarNuevaAfiliacion(solicitarAfiliacionDto.BarberiaId, solicitarAfiliacionDto.BarberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Solicitudes De Afiliacion del Barbero
        /// </summary>
        /// <param name="barberoId">Id del barbero</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerSolicitudesDeBarbero(Guid barberoId)
        {
            var result = await _SolicitudAfiliacionServicio.ObtenerSolicitudesDeBarbero(barberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Obtener Solicitud De Afiliacion activa del Barbero
        /// </summary>
        /// <param name="barberoId">Id del barbero</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerAfiliacionActivaDeBarbero(Guid barberoId)
        {
            var result = await _SolicitudAfiliacionServicio.ObtenerAfiliacionActivaDeBarbero(barberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }
        
    }
}
