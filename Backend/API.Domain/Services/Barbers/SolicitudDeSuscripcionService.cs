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
    public class SolicitudDeSuscripcionService : BasicService<SolicitudDeSuscripcion, SolicitudDeSuscripcionValidator>, ISolicitudDeSuscripcionService
    {

        public SolicitudDeSuscripcionService(IUnitOfWork<SolicitudDeSuscripcion> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public async Task<bool> SolicitarNuevaSuscripcion(Guid nuevaSuscripcionId, Guid? barberiaId, Guid? barberoId)
        {
            if (barberiaId.HasValue)
            {
                var existenSolicitudesPendientes = await _repositorios.SolicitudDeSuscripciones
                                                    .GetQuery()
                                                    .AnyAsync(e => e.BarberiaId == barberiaId && e.EstadoSuscripcion == EstadoSuscripcion.Pendiente);
                if (existenSolicitudesPendientes)
                {
                    throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "Ya usted posee una solicitud de suscripción en progreso." };
                }
                else
                {
                    var nuevaSolicitud = new SolicitudDeSuscripcion
                    {
                        Id = Guid.NewGuid(),
                        BarberoId = null,
                        BarberiaId = barberiaId,
                        FechaSolicitado = DateTime.Now,
                        EstadoSuscripcion = EstadoSuscripcion.Pendiente,
                        SuscripcionId = nuevaSuscripcionId

                    };
                    await _repositorios.SolicitudDeSuscripciones.AddAsync(nuevaSolicitud);
                }
            }
            else if (barberoId.HasValue)
            {
                var existenSolicitudesPendientes = await _repositorios.SolicitudDeSuscripciones
                                                    .GetQuery()
                                                    .AnyAsync(e => e.BarberoId == barberoId && e.EstadoSuscripcion == EstadoSuscripcion.Pendiente);
                if (existenSolicitudesPendientes)
                {
                    throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "Ya usted posee una solicitud de suscripción en progreso." };
                }
                else
                {
                    var nuevaSolicitud = new SolicitudDeSuscripcion
                    {
                        Id = Guid.NewGuid(),
                        BarberoId = barberoId,
                        BarberiaId = null,
                        FechaSolicitado = DateTime.Now,
                        EstadoSuscripcion = EstadoSuscripcion.Pendiente,
                        SuscripcionId = nuevaSuscripcionId

                    };
                    await _repositorios.SolicitudDeSuscripciones.AddAsync(nuevaSolicitud);
                }
            }
            else
            {
                throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "La solicitud debe ser enviada por una barbería o un barbero." };
            }

            await _repositorios.BasicRepository.SaveChangesAsync();
            return true;
        }

        public async Task AprobarSolicitud(Guid solicitudId)
        {
            var solicitud = await _repositorios.SolicitudDeSuscripciones
                                    .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.Id == solicitudId)
                                    ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." }; ;

            solicitud.FechaAprobado = DateTime.Now;

            var barberia = await _repositorios.Barberias
                                    .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.Id == solicitud.BarberiaId)
                                    ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." }; ;

            var tiempoVigencia = await _repositorios.Suscripciones
                                            .GetQuery()
                                            .AsNoTracking()
                                            .Where(e => e.Id == solicitud.SuscripcionId)
                                            .Select(e => e.TiempoVigencia)
                                            .FirstOrDefaultAsync();

            barberia.FechaVencimientoSuscripcion = ObtenerFechaVencimiento(tiempoVigencia, solicitud.FechaAprobado);

            _repositorios.SolicitudDeSuscripciones.Update(solicitud);
            _repositorios.Barberias.Update(barberia);
            await _repositorios.BasicRepository.SaveChangesAsync();
        }

        public static DateTime? ObtenerFechaVencimiento(int? tiempoVigencia, DateTime? fechaAprobado)
        {
            // Se requiere que la solicitud tenga fecha de aprobado y que la suscripción tenga tiempo de vigencia
            if (fechaAprobado == null || tiempoVigencia == null)
                return null;

            return fechaAprobado.Value.AddDays(tiempoVigencia.Value);
        }

        public async Task RechazarSolicitud(Guid solicitudId, string motivoRechazo)
        {
            var solicitud = await _repositorios.SolicitudDeSuscripciones
                                    .GetQuery()
                                    .AsTracking()
                                    .FirstOrDefaultAsync(e => e.Id == solicitudId)
                                    ?? throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." }; ;
            if (solicitud.BarberiaId.HasValue)
            {
                var barberia = await _repositorios.Barberias
                                        .GetQuery()
                                        .AsTracking()
                                        .FirstOrDefaultAsync(e => e.Id == solicitud.BarberiaId);
                barberia.EstadoSuscripcion = EstadoSuscripcion.Cancelada;
                _repositorios.Barberias.Update(barberia);
            }
            else
            {
                var barbero = await _repositorios.Barberos
                                           .GetQuery()
                                           .AsTracking()
                                           .FirstOrDefaultAsync(e => e.Id == solicitud.BarberoId);
                barbero.EstadoSuscripcion = EstadoSuscripcion.Cancelada;
                _repositorios.Barberos.Update(barbero);
            }
            solicitud.MotivoRechazo = motivoRechazo;
            _repositorios.SolicitudDeSuscripciones.Update(solicitud);
            await _repositorios.BasicRepository.SaveChangesAsync();
        }

    }
}