using API.Data.Entidades.Barbers;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class CaracteristicaSuscripcionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaracteristicaSuscripcion>().ToTable("CaracteristicaSuscripciones");
            EntidadBaseConfiguracionBD<CaracteristicaSuscripcion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<CaracteristicaSuscripcion>().Property(e => e.SuscripcionId).IsRequired();
            modelBuilder.Entity<CaracteristicaSuscripcion>().Property(e => e.Descripcion).IsRequired();

            #region Seed
            // Barbero
            // Suscripcion Free 
            CaracteristicaSuscripcion cs1 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336520"),
                Descripcion = "Visibilidad ante búsquedas de clientes",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336520"),
            };
            CaracteristicaSuscripcion cs2 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336521"),
                Descripcion = "Gestión de servicios",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336520"),
            };

            // Suscripcion Popular 

            CaracteristicaSuscripcion cs3 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
                Descripcion = "Visibilidad ante búsquedas de clientes",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336521"),
            };
            CaracteristicaSuscripcion cs4 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
                Descripcion = "Gestión de servicios",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336521"),
            };
            CaracteristicaSuscripcion cs5 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
                Descripcion = "Gestión de reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336521"),
            };
            CaracteristicaSuscripcion cs6 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336525"),
                Descripcion = "Acceso a estadísticas de clientes y reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336521"),
            };
            CaracteristicaSuscripcion cs7 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336526"),
                Descripcion = "Acceso a contabilidad",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336521"),
            };

            // Suscripcion Premium 

            CaracteristicaSuscripcion cs8 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336527"),
                Descripcion = "Visibilidad ante búsquedas de clientes",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
            };
            CaracteristicaSuscripcion cs9 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336528"),
                Descripcion = "Gestión de servicios",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
            };
            CaracteristicaSuscripcion cs10 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336529"),
                Descripcion = "Gestión de reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
            };
            CaracteristicaSuscripcion cs11 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336530"),
                Descripcion = "Acceso a estadísticas de clientes y reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
            };
            CaracteristicaSuscripcion cs12 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336531"),
                Descripcion = "Acceso a contabilidad",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
            };
            CaracteristicaSuscripcion cs13 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336532"),
                Descripcion = "Gestión de productos en venta",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
            };
            CaracteristicaSuscripcion cs14 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336533"),
                Descripcion = "Acceso a estadísticas de productos",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
            };

            // Barbería
            // Suscripcion Popular 

            CaracteristicaSuscripcion cs15 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336534"),
                Descripcion = "Visibilidad ante búsquedas de clientes",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
            };
            CaracteristicaSuscripcion cs16 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336535"),
                Descripcion = "Gestión de servicios",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
            };
            CaracteristicaSuscripcion cs17 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336536"),
                Descripcion = "Gestión de reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
            };
            CaracteristicaSuscripcion cs18 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336537"),
                Descripcion = "Acceso a estadísticas de clientes y reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
            };
            CaracteristicaSuscripcion cs19 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336538"),
                Descripcion = "Acceso a contabilidad",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
            };
            CaracteristicaSuscripcion cs20 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336539"),
                Descripcion = "Posibilidad de incorporar hasta 5 barberos",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336523"),
            };

            // Suscripcion Premium 

            CaracteristicaSuscripcion cs21 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336540"),
                Descripcion = "Visibilidad ante búsquedas de clientes",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };
            CaracteristicaSuscripcion cs22 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336541"),
                Descripcion = "Gestión de servicios",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };
            CaracteristicaSuscripcion cs23 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336542"),
                Descripcion = "Gestión de reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };
            CaracteristicaSuscripcion cs24 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336543"),
                Descripcion = "Acceso a estadísticas de clientes y reservas",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };
            CaracteristicaSuscripcion cs25 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336544"),
                Descripcion = "Acceso a contabilidad",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };
            CaracteristicaSuscripcion cs26 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336545"),
                Descripcion = "Gestión de productos en venta",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };
            CaracteristicaSuscripcion cs27 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336546"),
                Descripcion = "Acceso a estadísticas de productos",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };
            CaracteristicaSuscripcion cs28 = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336547"),
                Descripcion = "Posibilidad de incorporar hasta 15 barberos",
                SuscripcionId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336524"),
            };

            // Barbero
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs1);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs2);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs3);

            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs4);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs5);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs6);

            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs7);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs8);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs9);

            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs10);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs11);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs12);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs13);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs14);

            // Barbería
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs15);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs16);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs17);

            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs18);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs19);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs20);

            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs21);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs22);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs23);

            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs24);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs25);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs26);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs27);
            modelBuilder.Entity<CaracteristicaSuscripcion>().HasData(cs28);
            #endregion

        }
    }
}
