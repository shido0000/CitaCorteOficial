using API.Data.Entidades.Barbers;
using API.Data.Entidades.Nomencladores;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class BarberiaConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Barberia>().ToTable("Barberias");
            EntidadBaseConfiguracionBD<Barberia>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Barberia>().Property(e => e.UsuarioId).IsRequired();
            modelBuilder.Entity<Barberia>().Property(e => e.Nombre).IsRequired();
            modelBuilder.Entity<Barberia>().Property(e => e.Direccion).IsRequired();
            modelBuilder.Entity<Barberia>().Property(e => e.SuscripcionId).IsRequired();
            modelBuilder.Entity<Barberia>().Property(e => e.HorarioApertura).IsRequired().HasDefaultValue("9:00");
            modelBuilder.Entity<Barberia>().Property(e => e.HorarioCierre).IsRequired().HasDefaultValue("18:00");
            modelBuilder.Entity<Barberia>().Property(e => e.EstadoSuscripcion).IsRequired().HasDefaultValue(EstadoSuscripcion.Pendiente);

            modelBuilder.Entity<Barberia>().HasIndex(e => new { e.Nombre }).IsUnique();

            modelBuilder.Entity<Barberia>().HasOne(e => e.Suscripcion).WithMany(e => e.Barberias).HasForeignKey(e => e.SuscripcionId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Barberia>()
             .HasOne(b => b.Usuario)               // Barberia tiene un Usuario
             .WithOne(u => u.Barberia)             // Usuario tiene una Barberia (relación 1 a 1)
             .HasForeignKey<Barberia>(b => b.UsuarioId)  // Clave foránea en Barberia
             .OnDelete(DeleteBehavior.Restrict);   // Evita borrado en cascada
        }
    }
}
