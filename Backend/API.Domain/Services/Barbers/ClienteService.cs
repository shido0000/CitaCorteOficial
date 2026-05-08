using API.Data.Dtos.ClienteDto;
using API.Data.Dtos.ReservaDto;
using API.Data.Entidades.Barbers;
using API.Data.Enum;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Barbers
{
    public class ClienteService : BasicService<Cliente, ClienteValidator>, IClienteService
    {
        private IBarberiaService _barberiaService;
        public ClienteService(IUnitOfWork<Cliente> repositorios, IHttpContextAccessor httpContext, IBarberiaService barberiaService) : base(repositorios, httpContext)
        {
            _barberiaService = barberiaService;
        }

        public async Task<int> TotalReservas(Guid clienteId)
        {
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.ClienteId == clienteId)
                                    .CountAsync();
        }

        public async Task<int> TotalReservasConfirmadas(Guid clienteId)
        {
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.ClienteId == clienteId && e.EstadoReserva.HasValue && e.EstadoReserva == EstadoReserva.Completada)
                                    .CountAsync();
        }

        public async Task<List<ReservasPendienteDto>> ObtenerListadoReservasProximas(Guid clienteId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Servicio)
                                    .Include(e => e.Barberia)
                                    .Include(e => e.Barbero)
                                        .ThenInclude(e => e.Usuario)
                                    .Where(e => e.ClienteId == clienteId && e.Fecha.Value.Date >= hoy && e.EstadoReserva.HasValue && e.EstadoReserva == EstadoReserva.Aprobada)
                                    .Select(e => new ReservasPendienteDto
                                    {
                                        Id = e.Id,
                                        Fecha = e.Fecha.Value,
                                        Hora = e.Hora,
                                        NombreBarberiaBarbero = e.BarberiaId != null ? e.Barberia.Nombre : e.BarberoId != null ? e.Barbero.Usuario.NombreCompleto : "-",
                                        NombreServicio = e.Servicio.Nombre,
                                        Rating = 0,
                                        Resenhas = 0,
                                        Distancia = "-",
                                    })
                                    .ToListAsync();
        }

        public async Task<DatosDashboardClienteDto> ObtenerDatosDashBoardCliente(Guid clienteId)
        {
            var totalReservas = await TotalReservas(clienteId);
            var totalReservasCompletadas = await TotalReservasConfirmadas(clienteId);
            var listadoReservasProximas = await ObtenerListadoReservasProximas(clienteId);
            var totalReservasProximas = listadoReservasProximas.Count();
            var listadBarberiasRecomendadas = await _barberiaService.ObtenerBarberiasRecomendadas();

            return await _repositorios.Clientes
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.Id == clienteId)
                                    .Select(e => new DatosDashboardClienteDto
                                    {
                                        Id = e.Id,
                                        TotalReservas = totalReservas,
                                        TotalReservasCompletadas = totalReservasCompletadas,
                                        TotalReservasProximas = totalReservasProximas,
                                        ListadoReservasProximas = listadoReservasProximas,
                                        ListadBarberiasRecomendadas = listadBarberiasRecomendadas,
                                    })
                                    .FirstOrDefaultAsync()
                                    ?? new DatosDashboardClienteDto
                                    {
                                        Id = clienteId,
                                        TotalReservas = totalReservas,
                                        TotalReservasCompletadas = totalReservasCompletadas,
                                        TotalReservasProximas = totalReservasProximas,
                                        ListadoReservasProximas = listadoReservasProximas,
                                        ListadBarberiasRecomendadas = listadBarberiasRecomendadas,
                                    };
        }

    }
}