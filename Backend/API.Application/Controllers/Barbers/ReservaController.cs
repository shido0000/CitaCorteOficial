using API.Application.Dtos.Barbers.Reserva;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace API.Application.Controllers.Barbers
{
    public class ReservaController : BasicController<Reserva, ReservaValidator, DetallesReservaDto, CrearReservaInputDto, ActualizarReservaInputDto, ListadoPaginadoReservaDto, FiltrarConfigurarListadoPaginadoReservaIntputDto>
    {

        public ReservaController(IMapper mapper, IReservaService servicioReserva) : base(mapper, servicioReserva)
        {
        }

        protected override Task<(IEnumerable<Reserva>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoReservaIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Reserva, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                //filtros.Add(Reserva => Reserva.Nombre.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                //                       Reserva.Direccion.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }
            if (inputDto.BarberiaId.HasValue)
            {
                filtros.Add(elemento => elemento.BarberiaId == inputDto.BarberiaId.Value);
            }
            if (inputDto.BarberoId.HasValue)
            {
                filtros.Add(elemento => elemento.BarberoId == inputDto.BarberoId.Value);
            }
            if (inputDto.ClienteId.HasValue)
            {
                filtros.Add(elemento => elemento.ClienteId == inputDto.ClienteId.Value);
            }
            
            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

    }
}
