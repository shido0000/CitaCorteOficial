using API.Application.Dtos.Barbers.SolicitarSuscripcion;
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
    public class SolicitudSuscripcionController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ISolicitudDeSuscripcionService _SolicitudSuscripcionServicio;

        public SolicitudSuscripcionController(IMapper mapper, ISolicitudDeSuscripcionService SolicitudSuscripcionServicio)
        {
            _SolicitudSuscripcionServicio = SolicitudSuscripcionServicio;
            _mapper = mapper;
        }

        /// <summary>
        /// Solicitar Nueva Suscripcion
        /// </summary>
        /// <param name="solicitarSuscripcionDto">Elemento a crear</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> SolicitarNuevaSuscripcion([FromBody] SolicitarSuscripcionDto solicitarSuscripcionDto)
        {
            var result = await _SolicitudSuscripcionServicio.SolicitarNuevaSuscripcion(solicitarSuscripcionDto.NuevaSuscripcionId, solicitarSuscripcionDto.BarberiaId, solicitarSuscripcionDto.BarberoId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

        /// <summary>
        /// Rechazar Solicitud
        /// </summary>
        /// <param name="rechazarSuscripcionDto">Elemento</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> RechazarSolicitud([FromBody] RechazarSuscripcionDto rechazarSuscripcionDto)
        {
            await _SolicitudSuscripcionServicio.RechazarSolicitud(rechazarSuscripcionDto.SolicitudId, rechazarSuscripcionDto.MotivoRechazo);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK });
        }
    }
}
