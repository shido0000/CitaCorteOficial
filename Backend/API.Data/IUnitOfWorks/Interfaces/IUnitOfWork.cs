using API.Data.Entidades;
using API.Data.IUnitOfWorks.Interfaces.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Nomencladores;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Interfaces
{
    public interface IUnitOfWork<TEntity> : IDisposable where TEntity : EntidadBase
    {
        IPermisoRepository Permisos { get; }
        IRolPermisoRepository RolesPermisos { get; }
        IRolRepository Roles { get; }
        IUsuarioRepository Usuarios { get; }
        IBaseRepository<TEntity> BasicRepository { get; }
        ITrazaRepository Trazas { get; }

        ICategoriaRepository Categorias { get; }
        IOrigenRepository Origenes { get; }

        IGrupoRepository Grupos { get; }
        IFamiliaRepository Familias { get; }


        IBarberiaRepository Barberias { get; }
        IBarberoRepository Barberos { get; }

        IClienteRepository Clientes { get; }
        IComercialRepository Comerciales { get; }
        IHistoricoSuscripcionBarberoRepository HistoricoSuscripcionBarberos { get; }
        INotificacionRepository Notificaciones { get; }

        IProductoRepository Productos { get; }
        IReservaRepository Reservas { get; }
        IServicioRepository Servicios { get; }
        ISuscripcionRepository Suscripciones { get; }
        IMonedaRepository Monedas { get; }
        ICaracteristicaSuscripcionRepository CaracteristicaSuscripciones { get; }
        ISolicitudDeSuscripcionRepository SolicitudDeSuscripciones { get; }
        ISolicitudDeAfiliacionRepository SolicitudDeAfiliaciones { get; }
        IAdminRepository Admins { get; }
        IResenhaRepository Resenhas { get; }
        ICalificacionRepository Calificaciones { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
