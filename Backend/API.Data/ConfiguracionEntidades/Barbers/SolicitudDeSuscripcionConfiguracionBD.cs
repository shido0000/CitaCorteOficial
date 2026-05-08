using API.Data.Entidades.Barbers;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class SolicitudDeSuscripcionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolicitudDeSuscripcion>().ToTable("SolicitudDeSuscripciones");
            EntidadBaseConfiguracionBD<SolicitudDeSuscripcion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<SolicitudDeSuscripcion>().Property(e => e.FechaSolicitado).IsRequired();
            modelBuilder.Entity<SolicitudDeSuscripcion>().Property(e => e.SuscripcionId).IsRequired();
            modelBuilder.Entity<SolicitudDeSuscripcion>().Property(e => e.EstadoSuscripcion).IsRequired().HasDefaultValue(EstadoSuscripcion.Pendiente);

            modelBuilder.Entity<SolicitudDeSuscripcion>().HasOne(e => e.Suscripcion).WithMany(e => e.SolicitudDeSuscripciones).HasForeignKey(e => e.SuscripcionId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SolicitudDeSuscripcion>()
             .HasOne(b => b.Barbero)               // SolicitudDeSuscripcion tiene un Usuario
             .WithOne(u => u.SolicitudDeSuscripcion)             // Usuario tiene una SolicitudDeSuscripcion (relación 1 a 1)
             .HasForeignKey<SolicitudDeSuscripcion>(b => b.BarberoId)  // Clave foránea en SolicitudDeSuscripcion
             .OnDelete(DeleteBehavior.Restrict);   // Evita borrado en cascada

            modelBuilder.Entity<SolicitudDeSuscripcion>()
            .HasOne(b => b.Barberia)               // SolicitudDeSuscripcion tiene un Usuario
            .WithOne(u => u.SolicitudDeSuscripcion)             // Usuario tiene una SolicitudDeSuscripcion (relación 1 a 1)
            .HasForeignKey<SolicitudDeSuscripcion>(b => b.BarberiaId)  // Clave foránea en SolicitudDeSuscripcion
            .OnDelete(DeleteBehavior.Restrict);   // Evita borrado en cascada
        }
    }
}
