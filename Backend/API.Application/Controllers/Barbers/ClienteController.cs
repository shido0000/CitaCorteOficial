using API.Application.Dtos.Barbers.Cliente;
using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class ClienteController : BasicController<Cliente, ClienteValidator, DetallesClienteDto, CrearClienteInputDto, ActualizarClienteInputDto, ListadoPaginadoClienteDto, FiltrarConfigurarListadoPaginadoClienteIntputDto>
    {
        private IClienteService _clienteService;
        public ClienteController(IMapper mapper, IClienteService servicioCliente) : base(mapper, servicioCliente)
        {
            _clienteService = servicioCliente;
        }

        protected override Task<(IEnumerable<Cliente>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoClienteIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Cliente, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Cliente => Cliente.Usuario.NombreCompleto.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Cliente.Usuario.Correo.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Cliente.Usuario.Username.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        /// <summary>
        /// Obtener Datos DashBoard Cliente
        /// </summary>
        /// <param name="clienteId">Elemento a obtener</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerDatosDashBoardCliente(Guid clienteId)
        {
            var result = await _clienteService.ObtenerDatosDashBoardCliente(clienteId);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }



    }
}
