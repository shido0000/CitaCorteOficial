using API.Data.Entidades.Barbers;
using API.Data.Enum;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Barbers
{
    public class SolicitudDeAfiliacionService : BasicService<SolicitudDeAfiliacion, SolicitudDeAfiliacionValidator>, ISolicitudDeAfiliacionService
    {

        public SolicitudDeAfiliacionService(IUnitOfWork<SolicitudDeAfiliacion> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public async Task<bool> SolicitarNuevaAfiliacion(Guid barberiaId, Guid barberoId)
        {
            if (barberiaId == Guid.Empty || barberoId == Guid.Empty)
            {
                throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "Error en los datos de la solicitud." };
            }

            var existenSolicitudesPendientes = await _repositorios.SolicitudDeAfiliaciones
                                                .GetQuery()
                                                .AnyAsync(e => e.BarberoId == barberoId && e.EstadoSolicitudAfiliacion == EstadoSolicitudAfiliacion.Pendiente);
            if (existenSolicitudesPendientes)
            {
                throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "Ya usted posee una solicitud de afliliación en progreso." };
            }
            else
            {
                var nuevaSolicitud = new SolicitudDeAfiliacion
                {
                    Id = Guid.NewGuid(),
                    BarberoId = barberoId,
                    BarberiaId = barberiaId,
                    FechaSolicitado = DateTime.Now,
                    EstadoSolicitudAfiliacion = EstadoSolicitudAfiliacion.Pendiente,
                };
                await _repositorios.SolicitudDeAfiliaciones.AddAsync(nuevaSolicitud);
            }

            await _repositorios.BasicRepository.SaveChangesAsync();
            return true;
        }

        public async Task AprobarSolicitud(Guid solicitudId)
        {
            var solicitud = await _repositorios.SolicitudDeAfiliaciones
                                    .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.Id == solicitudId)
                                    ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." }; ;

            solicitud.FechaAprobado = DateTime.Now;

            var barbero = await _repositorios.Barberos
                                    .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.Id == solicitud.BarberoId)
                                    ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." }; ;

            barbero.BarberiaId = solicitud.BarberiaId;
            barbero.EstaAfiliadoABarberia = true;

            _repositorios.SolicitudDeAfiliaciones.Update(solicitud);
            _repositorios.Barberos.Update(barbero);
            await _repositorios.BasicRepository.SaveChangesAsync();
        }

        public async Task<List<SolicitudDeAfiliacion>> ObtenerSolicitudesDeBarbero(Guid barberoId)
        {
            return await _repositorios.SolicitudDeAfiliaciones
                                    .GetQuery()
                                    .AsTracking()
                                    .Where(e => e.BarberoId == barberoId)
                                    .ToListAsync();
        }

        public async Task<SolicitudDeAfiliacion?> ObtenerAfiliacionActivaDeBarbero(Guid barberoId)
        {
            return await _repositorios.SolicitudDeAfiliaciones
                                    .GetQuery()
                                    .AsTracking()
                                    .Where(e => e.BarberoId.HasValue && e.BarberoId.Value == barberoId)
                                    .FirstOrDefaultAsync();
        }
    }
}