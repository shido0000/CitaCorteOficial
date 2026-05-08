using API.Data.Entidades.Barbers;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class MonedaConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moneda>().ToTable("Monedas");
            EntidadBaseConfiguracionBD<Moneda>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Moneda>().Property(e => e.Codigo).IsRequired();
            modelBuilder.Entity<Moneda>().Property(e => e.Descripcion).IsRequired();
            modelBuilder.Entity<Moneda>().Property(e => e.TasaEnMN).IsRequired();

            modelBuilder.Entity<Moneda>().HasIndex(e => new { e.Codigo }).IsUnique();
            modelBuilder.Entity<Moneda>().HasIndex(e => new { e.Descripcion }).IsUnique();

            #region Seed

            Moneda m1 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
                Codigo = "CUP",
                Descripcion = "Moneda Nacional",
                TasaEnMN = 1m,
            };
            Moneda m2 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
                Codigo = "USD",
                Descripcion = "Dolar Americano",
                TasaEnMN = 520,
            };

            modelBuilder.Entity<Moneda>().HasData(m1);
            modelBuilder.Entity<Moneda>().HasData(m2);
            #endregion
        }
    }
}
