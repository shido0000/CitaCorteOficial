using API.Data.Dtos.BarberiaDto;
using API.Data.Dtos.BarberoDto;
using API.Data.Dtos.ServicioDto;
using API.Data.Dtos.SuscripcionDto;
using API.Data.Entidades.Barbers;
using API.Data.Enum;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace API.Domain.Services.Barbers
{
    public class BarberiaService : BasicService<Barberia, BarberiaValidator>, IBarberiaService
    {

        public BarberiaService(IUnitOfWork<Barberia> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public async Task<int> TotalReservasHoy(Guid barberiaId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.BarberiaId == barberiaId && e.Fecha.Value.Date == hoy)
                                    .CountAsync();
        }
        public async Task<int> TotaBarberosAfiliados(Guid barberiaId)
        {
            return await _repositorios.Barberos
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.BarberiaId == barberiaId && e.EstaAfiliadoABarberia.Value)
                                    .CountAsync();
        }
        public async Task<decimal> TotalIngresosHoy(Guid barberiaId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Servicio)
                                    .Where(e => e.BarberiaId == barberiaId && e.Fecha.Value.Date == hoy)
                                    .SumAsync(e => e.Servicio.Precio ?? 0m);
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
        public async Task<SuscripcionActualDto> ObtenerDatosSuscripcion(Guid barberiaId)
        {
            var hoy = DateTime.Today;
            var suscripcion = await _repositorios.Barberias
                                        .GetQuery()
                                        .AsNoTracking()
                                        .Where(e => e.Id == barberiaId)
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
            int cantidadBarberos = 0;
            foreach (var caract in suscripcion.Caracteristicas)
            {
                if (ExtraerNumeroDeDescripcion(caract) != 0)
                {
                    cantidadBarberos = ExtraerNumeroDeDescripcion(caract);
                    break;
                }
            }
            var suscripcionActualDto = new SuscripcionActualDto
            {
                NivelSuscripcion = suscripcion.NivelSuscripcion?.ToString() ?? NivelSuscripcion.Popular.ToString(),
                Nombre = suscripcion.Nombre ?? "-",
                Precio = suscripcion.Precio ?? 0m,
                TiempoVigencia = suscripcion.TiempoVigencia ?? 0,
                BarberosPermitidos = cantidadBarberos,
                Estado = suscripcion.EstadoSuscripcion?.ToString() ?? EstadoSuscripcion.Pendiente.ToString(),
                FechaVencimiento = suscripcion.FechaVencimientoSuscripcion?.Date ?? DateTime.Now,
                Caracteristicas = suscripcion.Caracteristicas,
            };
            return suscripcionActualDto;
        }


        public async Task<DatosDashboardBarberiaDto> ObtenerDatosDashBoardBarberia(Guid barberiaId)
        {
            var cantidadReservasHoy = await TotalReservasHoy(barberiaId);
            var cantidadBarberosAfiliados = await TotaBarberosAfiliados(barberiaId);
            var totalIngresosHoy = await TotalIngresosHoy(barberiaId);
            var suscripcion = await ObtenerDatosSuscripcion(barberiaId);

            return await _repositorios.Barberias
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.Id == barberiaId)
                                    .Select(e => new DatosDashboardBarberiaDto
                                    {
                                        Id = e.Id,
                                        CantidadReservasHoy = cantidadReservasHoy,
                                        CantidadBarberosAfiliados = cantidadBarberosAfiliados,
                                        IngresosHoy = totalIngresosHoy,
                                        Calificacion = 0,
                                        SuscripcionActualDto = suscripcion,
                                    })
                                    .FirstOrDefaultAsync()
                                    ?? new DatosDashboardBarberiaDto
                                    {
                                        Id = barberiaId,
                                        CantidadReservasHoy = 0,
                                        CantidadBarberosAfiliados = 0,
                                        IngresosHoy = 0m,
                                        Calificacion = 0,
                                        SuscripcionActualDto = suscripcion,
                                    };
        }

        public async Task<int> TotalReservas(Guid barberiaId)
        {
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Where(e => e.BarberiaId == barberiaId)
                                    .CountAsync();
        }

        public async Task<decimal> TotalIngresos(Guid barberiaId)
        {
            var hoy = DateTime.Today;
            return await _repositorios.Reservas
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Servicio)
                                    .Where(e => e.BarberiaId == barberiaId && e.Fecha.Value.Date <= hoy)
                                    .SumAsync(e => e.Servicio.Precio ?? 0m);
        }
        public async Task<int> TotalClientesUnicos(Guid barberiaId)
        {
            var hoy = DateTime.Today;
            var clientesUnicos = await _repositorios.Reservas
                .GetQuery()
                .AsNoTracking()
                .Where(r => r.BarberiaId == barberiaId && r.Fecha.Value.Date <= hoy && r.ClienteId != null)
                .Select(r => r.ClienteId)
                .Distinct()
                .CountAsync();

            return clientesUnicos;
        }
        public async Task<DatosEstadisticaBarberiaDto> ObtenerEstadisticasBarberia(Guid barberiaId)
        {
            var totalReservas = await TotalReservas(barberiaId);
            var totalIngresos = await TotalIngresos(barberiaId);
            var totalClientesUnicos = await TotalClientesUnicos(barberiaId);

            return new DatosEstadisticaBarberiaDto
            {
                Id = barberiaId,
                TotalReservas = totalReservas,
                Ingresos = totalIngresos,
                ClientesUnicos = totalClientesUnicos,
                CalificacionPromedio = 0
            };
        }

        public async Task<List<BarberiaRecomendadaDto>> ObtenerBarberiasRecomendadas()
        {
            List<BarberiaRecomendadaDto> recomendadas = new();
            var barberias = await _repositorios.Barberias
                                    .GetQuery()
                                    .AsNoTracking()
                                    .Include(e => e.Suscripcion)
                                    .Where(e => e.Suscripcion.NivelSuscripcion == NivelSuscripcion.Premium)
                                    .ToListAsync();

            var barberos = await _repositorios.Barberos
                                  .GetQuery()
                                  .AsNoTracking()
                                  .Include(e => e.Suscripcion)
                                  .Include(e => e.Usuario)
                                  .Where(e => e.Suscripcion.NivelSuscripcion == NivelSuscripcion.Premium)
                                  .ToListAsync();

            foreach (var barberia in barberias)
            {
                var nuevoRecomendada = new BarberiaRecomendadaDto
                {
                    Id = barberia.Id,
                    Clasificacion = 0,
                    Direccion = barberia.Direccion,
                    NombreBarberia = barberia.Nombre,
                    FotoUrl = barberia.LogoUrl
                };
                recomendadas.Add(nuevoRecomendada);
            }
            foreach (var barbero in barberos)
            {
                var nuevoRecomendada = new BarberiaRecomendadaDto
                {
                    Id = barbero.Id,
                    Clasificacion = 0,
                    Direccion = barbero.Direccion,
                    NombreBarberia = barbero.Usuario.Nombre,
                    FotoUrl = barbero.LogoUrl
                };
                recomendadas.Add(nuevoRecomendada);
            }

            return recomendadas;
        }

        public async Task<BarberiaDatosDto> ObtenerDatosBarberiaPorId(Guid barberiaId)
        {
            return await _repositorios.Barberias
                                       .GetQuery()
                                       .AsNoTracking()
                                       .Include(e => e.Servicios)
                                       .Include(e => e.Barberos)
                                        .ThenInclude(e => e.Usuario)
                                       .Where(e => e.Id == barberiaId)
                                       .Select(e => new BarberiaDatosDto
                                       {
                                           Id = e.Id,
                                           Direccion = e.Direccion ?? "-",
                                           FotoUrl = e.LogoUrl,
                                           HorarioApertura = e.HorarioApertura,
                                           HorarioCierre = e.HorarioCierre,
                                           NombreBarberia = e.Nombre ?? "-",
                                           Clasificacion = 0,
                                           Servicios = e.Servicios.Select(e => new ServicioDatosDto
                                           {
                                               Id = e.Id,
                                               Nombre = e.Nombre ?? "-",
                                               Descripcion = e.Descripcion ?? "-",
                                               Duracion = e.TiempoDemora ?? 1,
                                               Precio = e.Precio ?? 0m
                                           })
                                           .ToList(),
                                           Barberos = e.Barberos.Select(e => new BarberoDatosDto
                                           {
                                               Id = e.Id,
                                               Nombre = e.Usuario.Nombre ?? "-",
                                           })
                                           .ToList(),
                                       })
                                       .FirstOrDefaultAsync()
                                       ??
                                       throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
        }

        public async Task<List<SlotReservaDto>> ObtenerHorariosDisponiblesDetallados(
    Guid servicioId, DateTime fecha, Guid barberiaId, Guid barberoId)
        {
            // 1. Obtener barbería y servicio
            var barberia = await _repositorios.Barberias
                .GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == barberiaId);

            var servicio = await _repositorios.Servicios
                .GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == servicioId);

            if (barberia == null || servicio == null)
                throw new Exception("Barbería o servicio no encontrado");

            // 2. Parsear horario apertura/cierre
            var apertura = TimeSpan.Parse(barberia.HorarioApertura ?? "09:00");
            var cierre = TimeSpan.Parse(barberia.HorarioCierre ?? "18:00");

            // 3. Duración del servicio
            var duracion = servicio.TiempoDemora ?? 30;

            // 4. Generar slots
            var slots = new List<SlotReservaDto>();
            var horaActual = apertura;

            while (horaActual.Add(TimeSpan.FromMinutes(duracion)) <= cierre)
            {
                slots.Add(new SlotReservaDto
                {
                    Hora = horaActual.ToString(@"hh\:mm"),
                    DuracionMinutos = duracion,
                    Disponible = true, // se ajustará luego
                    ServicioId = servicioId,
                    BarberiaId = barberiaId,
                    BarberoId = barberoId
                });

                horaActual = horaActual.Add(TimeSpan.FromMinutes(duracion));
            }

            // 5. Obtener reservas existentes para ese barbero/barbería en esa fecha
            var reservas = await _repositorios.Reservas
                .GetQuery()
                .AsNoTracking()
                .Where(r => r.BarberiaId == barberiaId
                         && r.BarberoId == barberoId
                         && r.Fecha.HasValue
                         && r.Fecha.Value.Date == fecha.Date
                         && r.EstadoReserva == EstadoReserva.Aprobada)
                .Select(r => r.Hora)
                .ToListAsync();

            // 6. Marcar slots ocupados
            foreach (var slot in slots)
            {
                if (reservas.Contains(slot.Hora))
                {
                    slot.Disponible = false;
                }
            }

            return slots;
        }

    }
}