using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Barbers;
using API.Domain.Validators.Barbers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Barbers
{
    public class ServicioService : BasicService<Servicio, ServicioValidator>, IServicioService
    {

        public ServicioService(IUnitOfWork<Servicio> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public override async Task ValidarAntesCrear(Servicio entity)
        {
            var elementosExistentes = await _repositorios.Servicios
                                       .GetQuery()
                                       .AsNoTracking()
                                       .Select(e => new
                                       {
                                           Id = e.Id,
                                           Nombre = e.Nombre,
                                           Descripcion = e.Descripcion,
                                           BarberoId = e.BarberoId,
                                           BarberiaId = e.BarberiaId,
                                       })
                                       .ToListAsync();

            if (entity.BarberoId.HasValue)
            {
                elementosExistentes = elementosExistentes.Where(e => e.BarberoId.HasValue).ToList();

                var existe = elementosExistentes.Any(e => e.Nombre.ToLower().Equals(entity.Nombre.ToLower())
                                            && (e.BarberoId == entity.BarberoId));

                if (existe)
                {
                    throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con ese nombre asignado a ese barbero." };
                }
                else
                {
                    existe = elementosExistentes.Any(e => e.Descripcion.ToLower().Equals(entity.Descripcion.ToLower())
                                                && (e.BarberoId == entity.BarberoId));

                    if (existe)
                    {
                        throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con esa descripción asignado a ese barbero." };
                    }
                }
            }
            else if (entity.BarberiaId.HasValue)
            {
                elementosExistentes = elementosExistentes.Where(e => e.BarberiaId.HasValue).ToList();

                var existe = elementosExistentes.Any(e => e.Nombre.ToLower().Equals(entity.Nombre.ToLower())
                                            && (e.BarberiaId == entity.BarberiaId));

                if (existe)
                {
                    throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con ese nombre asignado a esa barbería." };
                }
                else
                {
                    existe = elementosExistentes.Any(e => e.Descripcion.ToLower().Equals(entity.Descripcion.ToLower())
                                                && (e.BarberiaId == entity.BarberiaId));

                    if (existe)
                    {
                        throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con esa descripción asignado a esa barbería." };
                    }
                }
            }
            else
            {
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "El servicio debe estar destinado a un barbero o una barbería." };
            }

            await base.ValidarAntesCrear(entity);
        }

        public override async Task ValidarAntesActualizar(Servicio entity)
        {
            var elementosExistentes = await _repositorios.Servicios
                                       .GetQuery()
                                       .AsNoTracking()
                                       .Where(e => e.Id != entity.Id)
                                       .Select(e => new
                                       {
                                           Id = e.Id,
                                           Nombre = e.Nombre,
                                           Descripcion = e.Descripcion,
                                           BarberoId = e.BarberoId,
                                           BarberiaId = e.BarberiaId,
                                       })
                                       .ToListAsync();

            if (entity.BarberoId.HasValue)
            {
                elementosExistentes = elementosExistentes.Where(e => e.BarberoId.HasValue).ToList();

                var existe = elementosExistentes.Any(e => e.Nombre.ToLower().Equals(entity.Nombre.ToLower())
                                            && (e.BarberoId == entity.BarberoId));

                if (existe)
                {
                    throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con ese nombre asignado a ese barbero." };
                }
                else
                {
                    existe = elementosExistentes.Any(e => e.Descripcion.ToLower().Equals(entity.Descripcion.ToLower())
                                                && (e.BarberoId == entity.BarberoId));

                    if (existe)
                    {
                        throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con esa descripción asignado a ese barbero." };
                    }
                }
            }
            else if (entity.BarberiaId.HasValue)
            {
                elementosExistentes = elementosExistentes.Where(e => e.BarberiaId.HasValue).ToList();

                var existe = elementosExistentes.Any(e => e.Nombre.ToLower().Equals(entity.Nombre.ToLower())
                                            && (e.BarberiaId == entity.BarberiaId));

                if (existe)
                {
                    throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con ese nombre asignado a esa barbería." };
                }
                else
                {
                    existe = elementosExistentes.Any(e => e.Descripcion.ToLower().Equals(entity.Descripcion.ToLower())
                                                && (e.BarberiaId == entity.BarberiaId));

                    if (existe)
                    {
                        throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Ya existe un servicio con esa descripción asignado a esa barbería." };
                    }
                }
            }
            else
            {
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "El servicio debe estar destinado a un barbero o una barbería." };
            }

            await base.ValidarAntesActualizar(entity);
        }

    }
}