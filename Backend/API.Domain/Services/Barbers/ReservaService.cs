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
    public class ReservaService : BasicService<Reserva, ReservaValidator>, IReservaService
    {

        public ReservaService(IUnitOfWork<Reserva> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public async Task<Guid> ConfirmarReserva(Guid reservaId)
        {
            var reservaExistente = await _repositorios.Reservas
                                            .GetQuery()
                                            .AsTracking()
                                            .FirstOrDefaultAsync(e => e.Id == reservaId) ??
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Reserva no encontrada." }; ;

            var usuarioId = await _repositorios.Clientes
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.Id == reservaExistente.ClienteId)
                                    .Select(e => e.UsuarioId)
                                    .FirstOrDefaultAsync();

            reservaExistente.EstadoReserva = EstadoReserva.Aprobada;

            var nuevaNotificacion = new Notificacion()
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                Titulo = "Confirmación de reserva",
                Mensaje = $"Su reserva ha sido confirmada para la fecha {reservaExistente.Fecha.Value.Date}",
                FueLeido = false,
                FechaCreado = DateTime.Now,
                FechaActualizado = DateTime.Now,
            };

            await _repositorios.Notificaciones.AddAsync(nuevaNotificacion);
            _repositorios.Reservas.Update(reservaExistente);
            await _repositorios.BasicRepository.SaveChangesAsync();
            return reservaExistente.Id;
        }

        public async Task<Guid> CancelarReserva(Guid reservaId, bool canceloCliente, Guid clienteTrabajadorId)
        {
            var reservaExistente = await _repositorios.Reservas
                                            .GetQuery()
                                            .AsTracking()
                                            .FirstOrDefaultAsync(e => e.Id == reservaId) ??
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Reserva no encontrada." }; ;

            var usuarioId = Guid.Empty;
            if (canceloCliente)
            {
                usuarioId = await _repositorios.Clientes
                                        .GetQuery()
                                        .AsNoTracking()
                                        .Where(e => e.Id == reservaExistente.ClienteId)
                                        .Select(e => e.UsuarioId)
                                        .FirstOrDefaultAsync();
            }
            else
            {
                if (reservaExistente.BarberoId.HasValue)
                {
                    usuarioId = await _repositorios.Barberos
                                               .GetQuery()
                                               .AsNoTracking()
                                               .Where(e => e.Id == reservaExistente.BarberoId)
                                               .Select(e => e.UsuarioId)
                                               .FirstOrDefaultAsync();
                }
                else
                {
                    usuarioId = await _repositorios.Barberias
                                                 .GetQuery()
                                                 .AsNoTracking()
                                                 .Where(e => e.Id == reservaExistente.BarberiaId)
                                                 .Select(e => e.UsuarioId)
                                                 .FirstOrDefaultAsync();
                }
            }

            reservaExistente.EstadoReserva = EstadoReserva.Cancelada;

            if (!canceloCliente)
            {
                var nuevaNotificacion = new Notificacion()
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    Titulo = "Cancelación de reserva",
                    Mensaje = $"Su reserva con fecha {reservaExistente.Fecha.Value.Date} ha sido cancelada por el cliente",
                    FueLeido = false,
                    FechaCreado = DateTime.Now,
                    FechaActualizado = DateTime.Now,
                };

                await _repositorios.Notificaciones.AddAsync(nuevaNotificacion);
            }
            else
            {
                var nuevaNotificacion = new Notificacion()
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    Titulo = "Cancelación de reserva",
                    Mensaje = $"Su reserva con fecha {reservaExistente.Fecha.Value.Date} ha sido cancelada",
                    FueLeido = false,
                    FechaCreado = DateTime.Now,
                    FechaActualizado = DateTime.Now,
                };

                await _repositorios.Notificaciones.AddAsync(nuevaNotificacion);
            }
            _repositorios.Reservas.Update(reservaExistente);
            await _repositorios.BasicRepository.SaveChangesAsync();
            return reservaExistente.Id;
        }

    }
}