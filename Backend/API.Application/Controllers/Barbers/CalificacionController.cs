using API.Application.Dtos.Barbers.Calificacion;
using API.Application.Dtos.Comunes;
using API.Data.Entidades.Barbers;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Barbers
{
    public class CalificacionController : BasicController<Calificacion, CalificacionValidator, DetallesCalificacionDto, CrearCalificacionInputDto, ActualizarCalificacionInputDto, ListadoPaginadoCalificacionDto, FiltrarConfigurarListadoPaginadoCalificacionIntputDto>
    {
        public CalificacionController(IMapper mapper, ICalificacionService servicioCalificacion) : base(mapper, servicioCalificacion)
        {
        }

        protected override Task<(IEnumerable<Calificacion>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoCalificacionIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Calificacion, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(Calificacion => Calificacion.Usuario.NombreCompleto.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Calificacion.Usuario.Correo.ToLower().Contains(inputDto.TextoBuscar.ToLower()) ||
                                       Calificacion.Usuario.Username.ToLower().Contains(inputDto.TextoBuscar.ToLower()));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }
    }
}
