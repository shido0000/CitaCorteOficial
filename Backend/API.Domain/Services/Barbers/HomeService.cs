using API.Data.Dtos.HomeDto;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Barbers;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Barbers
{
    public class HomeService : IHomeService
    {
        private IUnitOfWork<Barberia> _repositorios;
        public HomeService(IUnitOfWork<Barberia> repositorios)
        {
            _repositorios = repositorios;
        }

        public async Task<List<DatosBarberiasBarberosHomeDto>> ObtenerBarberiasYBarberosHome(string? texto)
        {
            List<DatosBarberiasBarberosHomeDto> listaRetorno = new();

            var barberias = await _repositorios.Barberias
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Select(e => new DatosBarberiasBarberosHomeDto
                                    {
                                        NombreBarberia = e.Nombre ?? "-",
                                        Telefono = e.Telefono ?? "-",
                                        Calificacion = 0m,
                                        Direccion = e.Direccion ?? "-",
                                        HorarioApertura = e.HorarioApertura ?? "-",
                                        HorarioCierre = e.HorarioCierre ?? "-",
                                        LogoUrl = e.LogoUrl ?? "-",
                                    })
                                    .ToListAsync();


            var barberos = await _repositorios.Barberos
                                   .GetQuery()
                                   .AsNoTracking()
                                   .Where(e => e.EstaAfiliadoABarberia.HasValue && !e.EstaAfiliadoABarberia.Value)
                                   .Include(e => e.Usuario)
                                   .Include(e => e.Suscripcion)
                                   .Include(e => e.SolicitudDeSuscripcion)
                                   .Select(e => new DatosBarberiasBarberosHomeDto
                                   {
                                       NombreBarberia = e.Usuario.NombreCompleto ?? "-",
                                       Direccion = e.Direccion,
                                       Telefono = e.Telefono ?? "-",
                                       HorarioApertura = e.HorarioApertura ?? "-",
                                       HorarioCierre = e.HorarioCierre ?? "-",
                                       LogoUrl = e.LogoUrl ?? "-",
                                       Calificacion = 0m,
                                   })
                                   .ToListAsync();

            listaRetorno.AddRange(barberias);
            listaRetorno.AddRange(barberos);

            if (!string.IsNullOrEmpty(texto))
            {
                texto = texto.ToLower();
                listaRetorno = listaRetorno.Where(e => e.NombreBarberia.ToLower() == texto).ToList();
            }

            return listaRetorno;
        }

    }
}