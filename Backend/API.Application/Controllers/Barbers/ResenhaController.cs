using API.Application.Dtos.Barbers.Resenha;
using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class ResenhaController : BasicController<Resenha, ResenhaValidator, DetallesResenhaDto, CrearResenhaInputDto, ActualizarResenhaInputDto, ListadoPaginadoResenhaDto, FiltrarConfigurarListadoPaginadoResenhaIntputDto>
    {
        public ResenhaController(IMapper mapper, IResenhaService servicioResenha) : base(mapper, servicioResenha)
        {
        }

        protected override Task<(IEnumerable<Resenha>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoResenhaIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Resenha, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Resenha => Resenha.Usuario.NombreCompleto.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Resenha.Usuario.Correo.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Resenha.Usuario.Username.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }
    }
}
