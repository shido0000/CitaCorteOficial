using API.Data.Entidades.Barbers;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class SuscripcionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Suscripcion>().ToTable("Suscripciones");
            EntidadBaseConfiguracionBD<Suscripcion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Suscripcion>().Property(e => e.TipoSuscripcion).IsRequired();
            modelBuilder.Entity<Suscripcion>().Property(e => e.NivelSuscripcion).IsRequired();
            modelBuilder.Entity<Suscripcion>().Property(e => e.Nombre).IsRequired();
            modelBuilder.Entity<Suscripcion>().Property(e => e.Descripcion).IsRequired();
            modelBuilder.Entity<Suscripcion>().Property(e => e.Precio).IsRequired().HasPrecision(18, 2);
            modelBuilder.Entity<Suscripcion>().Property(e => e.MonedaId).IsRequired();
            modelBuilder.Entity<Suscripcion>().Property(e => e.TiempoVigencia).IsRequired();
            modelBuilder.Entity<Suscripcion>().Property(e => e.EsFree).IsRequired().HasDefaultValue(false);

            modelBuilder.Entity<Suscripcion>().HasIndex(e => new { e.TipoSuscripcion, e.Nombre }).IsUnique();

            #region Seed

            Suscripcion sb1 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336520"),
                Nombre = "Free",
                Descripcion = "Solamente será visible para mundo pero no podra recibir reservas",
                Precio = 0m,
                EsFree = true,
                TiempoVigencia = 365,
                TipoSuscripcion = TipoSuscripcion.Barbero,
                NivelSuscripcion = NivelSuscripcion.Free,
                MonedaId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
            };
            Suscripcion sb2 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336521"),
                Nombre = "Popular",
                Descripcion = "Tendrá visibilidad, podrá recibir reservas, ver estadísticas, así como manejar su contabilidad",
                Precio = 5m,
                EsFree = false,
                TiempoVigencia = 30,
                TipoSuscripcion = TipoSuscripcion.Barbero,
                NivelSuscripcion = NivelSuscripcion.Popular,
                MonedaId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523")
            };
            Suscripcion sb3 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
                Nombre = "Premium",
                Descripcion = "Tendrá visibilidad, podrá recibir reservas, ver estadísticas, manejar su contabilidad, además de postear productos para vender",
                Precio = 10m,
                EsFree = true,
                TiempoVigencia = 30,
                TipoSuscripcion = TipoSuscripcion.Barbero,
                NivelSuscripcion = NivelSuscripcion.Premium,
                MonedaId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523")
            };

            // Barberias

            Suscripcion sbar1 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
                Nombre = "Popular",
                Descripcion = "Tendrá visibilidad, podrá recibir reservas, ver estadísticas, así como manejar su contabilidad",
                Precio = 10m,
                EsFree = false,
                TiempoVigencia = 30,
                TipoSuscripcion = TipoSuscripcion.Barberia,
                NivelSuscripcion = NivelSuscripcion.Popular,
                MonedaId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523")
            };
            Suscripcion sbar2 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
                Nombre = "Premium",
                Descripcion = "Tendrá visibilidad, podrá recibir reservas, ver estadísticas, manejar su contabilidad, además de postear productos para vender",
                Precio = 20m,
                EsFree = true,
                TiempoVigencia = 30,
                TipoSuscripcion = TipoSuscripcion.Barberia,
                NivelSuscripcion = NivelSuscripcion.Premium,
                MonedaId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523")
            };


            modelBuilder.Entity<Suscripcion>().HasData(sb1);
            modelBuilder.Entity<Suscripcion>().HasData(sb2);
            modelBuilder.Entity<Suscripcion>().HasData(sb3);
            modelBuilder.Entity<Suscripcion>().HasData(sbar1);
            modelBuilder.Entity<Suscripcion>().HasData(sbar2);


            #endregion
        }
    }
}
