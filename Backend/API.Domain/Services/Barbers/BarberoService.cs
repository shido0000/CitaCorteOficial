using API.Data.Dtos.BarberiaDto;
using API.Data.Dtos.BarberoDto;
using API.Data.Dtos.ReservaDto;
using API.Data.Dtos.SuscripcionDto;
using API.Data.Entidades.Barbers;
using API.Data.Enum;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace API.Domain.Services.Barbers
{
    public class BarberoService : BasicService<Barbero, BarberoValidator>, IBarberoService
    {

        public BarberoService(IUnitOfWork<Barbero> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public async Task<BarberosAfiliadosBarberiaDto> ObtenerBarberosAfiliadosABarberia(Guid barberiaId)
        {
            var barberos = await _repositorios.Barberos
                                        .GetQuery()
                                        .AsNoTracking()
                                        .Where(e => e.BarberiaId == barberiaId && e.EstaAfiliadoABarberia.HasValue && e.EstaAfiliadoABarberia.Value)
                                        .ToListAsync();

            var caracteristicas = await _repositorios.Barberias
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Suscripcion)
                                        .ThenInclude(e => e.CaracteristicaSuscripciones)
                                    .Where(e => e.Id == barberiaId)
                                    .Select(e => e.Suscripcion.CaracteristicaSuscripciones.Select(e => e.Descripcion).ToList())
                                    .FirstOrDefaultAsync();

            int cantidadBarberos = 0;
            foreach (var caract in caracteristicas)
            {
                if (ExtraerNumeroDeDescripcion(caract) != 0)
                {
                    cantidadBarberos = ExtraerNumeroDeDescripcion(caract);
                    break;
                }
            }

            var solicitudesPendientes = await _repositorios.SolicitudDeAfiliaciones
                                       .GetQuery()
                                       .AsNoTracking()
                                       .Where(e => e.BarberiaId == barberiaId && e.EstadoSolicitudAfiliacion == EstadoSolicitudAfiliacion.Pendiente)
                                       .ToListAsync();

            return new BarberosAfiliadosBarberiaDto
            {
                Id = barberiaId,
                CantidadBarberosAfiliados = barberos.Count(),
                CuposDisponibles = cantidadBarberos - barberos.Count(),
                SolicitudesPendientes = solicitudesPendientes,
                Barberos = barberos
            };
        }

        public static int ExtraerNumeroDeDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                return 0;

            // Busca "hasta 5" (insensible a mayúsculas/minúsculas)
            Match match = Regex.Match(descripcion, @"hasta\s+(\d+)", RegexOptions.IgnoreCase);
            if (match.Success)
                return int.Parse(match.Groups[1].Value);

            // Si no encuentra el patrón anterior, busca cualquier número
            match = Regex.Match(descripcion, @"\d+");
            if (match.Success)
                return int.Parse(match.Value);

            return 0;
        }

        public async Task<int> TotalReservas(Guid barberoId)
        {
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.BarberoId == barberoId)
                                    .CountAsync();
        }

        public async Task<decimal> TotalIngresos(Guid barberoId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Servicio)
                                    .Where(e => e.BarberoId == barberoId && e.Fecha.Value.Date <= hoy)
                                    .SumAsync(e => e.Servicio.Precio ?? 0m);
        }
        public async Task<int> TotalClientesUnicos(Guid barberoId)
        {
            var hoy = DateTime.Today;
            var clientesUnicos = await _repositorios.Reservas
                .GetQuery()
                .AsNoTracking()
                .Where(r => r.BarberoId == barberoId && r.Fecha.Value.Date <= hoy && r.ClienteId != null)
                .Select(r => r.ClienteId)
                .Distinct()
                .CountAsync();

            return clientesUnicos;
        }
        public async Task<int> TotalServiciosEnElMes(Guid barberoId)
        {
            var hoy = DateTime.Today;
            var primerDiaMes = new DateTime(hoy.Year, hoy.Month, 1);

            var serviciosUnicos = await _repositorios.Reservas
                .GetQuery()
                .AsNoTracking()
                .Where(r => r.BarberoId == barberoId &&
                            r.Fecha.Value.Date >= primerDiaMes &&
                            r.Fecha.Value.Date <= hoy &&
                            r.ServicioId != null)
                .Select(r => r.ServicioId)
                .CountAsync();

            return serviciosUnicos;
        }

        public async Task<List<ServiciosMasSolicitadosBarberoDto>> ObtenerServiciosMasSolicitados(Guid barberoId)
        {
            var hoy = DateTime.Today;
            var resultados = await _repositorios.Reservas
                .GetQuery()
                .AsNoTracking()
                .Where(r => r.BarberoId == barberoId && r.Fecha.Value.Date <= hoy && r.ServicioId != null)
                .GroupBy(r => r.ServicioId)
                .Select(g => new ServiciosMasSolicitadosBarberoDto
                {
                    Nombre = g.FirstOrDefault().Servicio.Nombre, // O usar Join con Servicios
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .ToListAsync();

            return resultados;
        }

        public async Task<DatosEstadisticaBarberoDto> ObtenerEstadisticasBarbero(Guid barberoId)
        {
            var totalReservas = await TotalReservas(barberoId);
            var totalIngresos = await TotalIngresos(barberoId);
            var totalClientesUnicos = await TotalClientesUnicos(barberoId);
            var totalServiciosEnMes = await TotalServiciosEnElMes(barberoId);
            var serviciosMasSolicitados = await ObtenerServiciosMasSolicitados(barberoId);

            return new DatosEstadisticaBarberoDto
            {
                Id = barberoId,
                TotalReservas = totalReservas,
                Ingresos = totalIngresos,
                ClientesUnicos = totalClientesUnicos,
                TotalServiciosEnMes = totalServiciosEnMes,
                ServiciosMasSolicitadosBarberoDto = serviciosMasSolicitados,
                CalificacionPromedio = 0
            };
        }

        public async Task<int> TotalReservasHoy(Guid barberoId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.BarberoId == barberoId && e.Fecha.Value.Date == hoy)
                                    .CountAsync();
        }

        public async Task<int> TotalReservasCompletadasHoy(Guid barberoId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.BarberoId == barberoId && e.Fecha.Value.Date == hoy && e.EstadoReserva == EstadoReserva.Completada)
                                    .CountAsync();
        }

        public async Task<decimal> TotalIngresosHoy(Guid barberoId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Servicio)
                                    .Where(e => e.BarberoId == barberoId && e.Fecha.Value.Date == hoy && e.EstadoReserva == EstadoReserva.Completada)
                                    .SumAsync(e => e.Servicio.Precio ?? 0m);
        }

        public async Task<SuscripcionActualBarberoDto> ObtenerDatosSuscripcion(Guid barberoId)
        {
            var hoy = DateTime.Today;
            var suscripcion = await _repositorios.Barberos
                                        .GetQuery()
                                        .AsNoTracking()
                                        .Where(e => e.Id == barberoId)
                                        .Include(e => e.Suscripcion)
                                         .ThenInclude(e => e.CaracteristicaSuscripciones)
                                        .Select(e => new
                                        {
                                            NivelSuscripcion = e.Suscripcion.NivelSuscripcion,
                                            Nombre = e.Suscripcion.Nombre,
                                            Precio = e.Suscripcion.Precio,
                                            TiempoVigencia = e.Suscripcion.TiempoVigencia,
                                            EstadoSuscripcion = e.EstadoSuscripcion,
                                            FechaVencimientoSuscripcion = e.FechaVencimientoSuscripcion,
                                            Caracteristicas = e.Suscripcion.CaracteristicaSuscripciones.Select(e => e.Descripcion).ToList(),

                                        })
                                        .FirstOrDefaultAsync();

            var suscripcionActualDto = new SuscripcionActualBarberoDto
            {
                NivelSuscripcion = suscripcion.NivelSuscripcion?.ToString() ?? NivelSuscripcion.Popular.ToString(),
                Nombre = suscripcion.Nombre ?? "-",
                Precio = suscripcion.Precio ?? 0m,
                TiempoVigencia = suscripcion.TiempoVigencia ?? 0,
                Estado = suscripcion.EstadoSuscripcion?.ToString() ?? EstadoSuscripcion.Pendiente.ToString(),
                FechaVencimiento = suscripcion.FechaVencimientoSuscripcion?.Date ?? DateTime.Now,
                Caracteristicas = suscripcion.Caracteristicas,
            };
            return suscripcionActualDto;
        }

        public async Task<List<ReservasHoyDto>> ObtenerDatosReservaHoy(Guid barberoId)
        {
            var hoy = DateTime.Today;
            var reservas = await _repositorios.Reservas
                                        .GetQuery()
                                        .AsNoTracking()
                                        .Where(e => e.BarberoId == barberoId && e.Fecha.HasValue && e.Fecha.Value.Date == hoy)
                                        .Include(e => e.Servicio)
                                        .Include(e => e.Cliente)
                                            .ThenInclude(e => e.Usuario)
                                        .Select(e => new ReservasHoyDto
                                        {
                                            Id = e.Id,
                                            PrecioServicio = e.Servicio.Precio ?? 0m,
                                            EstadoDeReserva = e.EstadoReserva.Value.ToString() ?? EstadoReserva.Pendiente.ToString(),
                                            NombreServicio = e.Servicio.Nombre,
                                            FechaReserva = e.Fecha.Value,
                                            DuracionEnMinutos = e.Servicio.TiempoDemora ?? 1,
                                            NombreCliente = e.Cliente.Usuario.NombreCompleto,

                                        })
                                        .ToListAsync();
            return reservas;
        }

        public async Task<DatosDashboardBarberoDto> ObtenerDatosDashBoardBarbero(Guid barberoId)
        {
            var cantidadReservasHoy = await TotalReservasHoy(barberoId);
            var cantidadReservasCompletadasHoy = await TotalReservasCompletadasHoy(barberoId);
            var totalIngresosHoy = await TotalIngresosHoy(barberoId);
            var suscripcion = await ObtenerDatosSuscripcion(barberoId);
            var reservaParaHoy = await ObtenerDatosReservaHoy(barberoId);

            return await _repositorios.Barberos
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.Id == barberoId)
                                    .Select(e => new DatosDashboardBarberoDto
                                    {
                                        Id = e.Id,
                                        CantidadReservasHoy = cantidadReservasHoy,
                                        CantidadReservasCompletadasHoy = cantidadReservasCompletadasHoy,
                                        IngresosHoy = totalIngresosHoy,
                                        Calificacion = 0,
                                        SuscripcionActualDto = suscripcion,
                                        ReservasParaHoy = reservaParaHoy,
                                    })
                                    .FirstOrDefaultAsync()
                                    ?? new DatosDashboardBarberoDto
                                    {
                                        Id = barberoId,
                                        CantidadReservasHoy = 0,
                                        CantidadReservasCompletadasHoy = 0,
                                        IngresosHoy = 0m,
                                        Calificacion = 0,
                                        SuscripcionActualDto = suscripcion,
                                        ReservasParaHoy = reservaParaHoy,
                                    };
        }

    }
}