using API.Data.DbContexts;
using API.Data.Entidades;
using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using API.Data.IUnitOfWorks.Interfaces.Barbers;
using API.Data.IUnitOfWorks.Interfaces.Nomencladores;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;
using API.Data.IUnitOfWorks.Repositorios;
using API.Data.IUnitOfWorks.Repositorios.Barbers;
using API.Data.IUnitOfWorks.Repositorios.Nomencladores;
using API.Data.IUnitOfWorks.Repositorios.Seguridad;

namespace API.Data.IUnitOfWorks
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : EntidadBase
    {
        private readonly ApiDbContext _context;

        public IPermisoRepository Permisos { get; }
        public IRolPermisoRepository RolesPermisos { get; }
        public IRolRepository Roles { get; }
        public IUsuarioRepository Usuarios { get; }
        public ITrazaRepository Trazas { get; }
        public IBaseRepository<TEntity> BasicRepository { get; }

        public ICategoriaRepository Categorias { get; }
        public IOrigenRepository Origenes { get; }
        public IGrupoRepository Grupos { get; }

        public IFamiliaRepository Familias { get; }

        public IBarberiaRepository Barberias { get; }
        public IBarberoRepository Barberos { get; }

        public IClienteRepository Clientes { get; }
        public IComercialRepository Comerciales { get; }
        public IHistoricoSuscripcionBarberoRepository HistoricoSuscripcionBarberos { get; }
        public INotificacionRepository Notificaciones { get; }

        public IProductoRepository Productos { get; }
        public IReservaRepository Reservas { get; }
        public IServicioRepository Servicios { get; }
        public ISuscripcionRepository Suscripciones { get; }
        public IMonedaRepository Monedas { get; }
        public ICaracteristicaSuscripcionRepository CaracteristicaSuscripciones { get; }
        public ISolicitudDeSuscripcionRepository SolicitudDeSuscripciones { get; }
        public ISolicitudDeAfiliacionRepository SolicitudDeAfiliaciones { get; }
        public IAdminRepository Admins { get; }
        public IResenhaRepository Resenhas { get; }
        public ICalificacionRepository Calificaciones { get; }

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Permisos = new PermisoRepository(context);
            RolesPermisos = new RolPermisoRepository(context);
            Roles = new RolRepository(context);
            Usuarios = new UsuarioRepository(context);
            Trazas = new TrazaRepository(context);

            Categorias = new CategoriaRepository(context);
            Origenes = new OrigenRepository(context);
            Grupos = new GrupoRepository(context);
            Familias = new FamiliaRepository(context);

            Barberias = new BarberiaRepository(context);
            Barberos = new BarberoRepository(context);
            Clientes = new ClienteRepository(context);
            Comerciales = new ComercialRepository(context);

            HistoricoSuscripcionBarberos = new HistoricoSuscripcionBarberoRepository(context);
            Notificaciones = new NotificacionRepository(context);
            Productos = new ProductoRepository(context);
            Reservas = new ReservaRepository(context);

            Servicios = new ServicioRepository(context);
            Suscripciones = new SuscripcionRepository(context);
            Monedas = new MonedaRepository(context);
            CaracteristicaSuscripciones = new CaracteristicaSuscripcionRepository(context);
            SolicitudDeSuscripciones = new SolicitudDeSuscripcionRepository(context);
            SolicitudDeAfiliaciones = new SolicitudDeAfiliacionRepository(context);
            Admins = new AdminRepository(context);
            Resenhas = new ResenhaRepository(context);
            Calificaciones = new CalificacionRepository(context);

            BasicRepository = new BaseRepository<TEntity>(context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
           => await _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context.Dispose();


    }
}
