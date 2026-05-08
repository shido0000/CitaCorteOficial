using API.Data.ConfiguracionEntidades.Barbers;
using API.Data.ConfiguracionEntidades.Nomencladores;
using API.Data.ConfiguracionEntidades.Seguridad;
using API.Data.Entidades.Barbers;
using API.Data.Entidades.Nomencladores;
using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DbContexts
{
    public class ApiDbContext : DbContext, IApiDbContext
    {
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermiso { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Traza> Trazas { get; set; }


        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Origen> Origenes { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Familia> Familias { get; set; }

        public DbSet<Barberia> Barberias { get; set; }
        public DbSet<Barbero> Barberos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Comercial> Comerciales { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<HistoricoSuscripcionBarbero> HistoricoSuscripcionBarberos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Suscripcion> Suscripciones { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<CaracteristicaSuscripcion> CaracteristicaSuscripciones { get; set; }
        public DbSet<SolicitudDeSuscripcion> SolicitudDeSuscripcion { get; set; }
        public DbSet<SolicitudDeAfiliacion> SolicitudDeAfiliacion { get; set; }
        public DbSet<Resenha> Resenhas { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RolPermisoConfiguracionBD.SetEntityBuilder(modelBuilder);
            RolConfiguracionBD.SetEntityBuilder(modelBuilder);
            PermisoConfiguracionBD.SetEntityBuilder(modelBuilder);
            UsuarioConfiguracionBD.SetEntityBuilder(modelBuilder);
            TrazaConfiguracionBD.SetEntityBuilder(modelBuilder);


            CategoriaConfiguracionBD.SetEntityBuilder(modelBuilder);
            OrigenConfiguracionBD.SetEntityBuilder(modelBuilder);
            GrupoConfiguracionBD.SetEntityBuilder(modelBuilder);
            FamiliaConfiguracionBD.SetEntityBuilder(modelBuilder);

            BarberiaConfiguracionBD.SetEntityBuilder(modelBuilder);
            BarberoConfiguracionBD.SetEntityBuilder(modelBuilder);
            HistoricoSuscripcionBarberoConfiguracionBD.SetEntityBuilder(modelBuilder);
            NotificacionConfiguracionBD.SetEntityBuilder(modelBuilder);
            ProductoConfiguracionBD.SetEntityBuilder(modelBuilder);
            ReservaConfiguracionBD.SetEntityBuilder(modelBuilder);
            ServicioConfiguracionBD.SetEntityBuilder(modelBuilder);
            SuscripcionConfiguracionBD.SetEntityBuilder(modelBuilder);
            MonedaConfiguracionBD.SetEntityBuilder(modelBuilder);
            CaracteristicaSuscripcionConfiguracionBD.SetEntityBuilder(modelBuilder);
            SolicitudDeSuscripcionConfiguracionBD.SetEntityBuilder(modelBuilder);
            SolicitudDeAfiliacionConfiguracionBD.SetEntityBuilder(modelBuilder);
            ResenhaConfiguracionBD.SetEntityBuilder(modelBuilder);
            CalificacionConfiguracionBD.SetEntityBuilder(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
