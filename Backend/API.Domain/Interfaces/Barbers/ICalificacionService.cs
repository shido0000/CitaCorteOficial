using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface ICalificacionService : IBaseService<Calificacion, CalificacionValidator>
    {

    }
}