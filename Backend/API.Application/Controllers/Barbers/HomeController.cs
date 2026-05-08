using API.Application.Dtos.Comunes;
using API.Application.Filters;
using API.Domain.Interfaces.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers.Barbers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerFilter))]
    public class HomeController : ControllerBase
    {
        private IHomeService _HomeService;
        public HomeController(IMapper mapper, IHomeService servicioHome)
        {
            _HomeService = servicioHome;
        }

        /// <summary>
        /// Obtener Barberias Y Barberos
        /// </summary>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerBarberiasYBarberos(string? texto)
        {
            var result = await _HomeService.ObtenerBarberiasYBarberosHome(texto);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

    }
}
