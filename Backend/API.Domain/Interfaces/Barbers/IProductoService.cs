using API.Data.Entidades.Barbers;
using API.Domain.Validators.Barbers;

namespace API.Domain.Interfaces.Barbers
{
    public interface IProductoService : IBaseService<Producto, ProductoValidator>
    {

    }
}