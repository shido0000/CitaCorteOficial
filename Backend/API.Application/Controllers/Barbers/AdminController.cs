using API.Application.Dtos.Barbers.Admin;
using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class AdminController : BasicController<Admin, AdminValidator, DetallesAdminDto, CrearAdminInputDto, ActualizarAdminInputDto, ListadoPaginadoAdminDto, FiltrarConfigurarListadoPaginadoAdminIntputDto>
    {
        private ISolicitudDeSuscripcionService _solicitudDeSuscripcionService;
        private IAdminService _AdminService;
        public AdminController(IMapper mapper, IAdminService servicioAdmin, ISolicitudDeSuscripcionService solicitudDeSuscripcionService) : base(mapper, servicioAdmin)
        {
            _solicitudDeSuscripcionService = solicitudDeSuscripcionService;
            _AdminService = servicioAdmin;
        }

        protected override Task<(IEnumerable<Admin>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoAdminIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Admin, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Admin => Admin.Usuario.NombreCompleto.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Admin.Usuario.Correo.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Admin.Usuario.Username.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
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
        /// Obtener Datos DashBoard Admin
        /// </summary>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerDatosDashBoardAdmin()
        {
            var result = await _AdminService.ObtenerDatosDashBoardAdmin();
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
            var result = await _AdminService.ObtenerBarberiasYBarberos();
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }

    }
}
