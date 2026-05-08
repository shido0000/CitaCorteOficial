using API.Data.Entidades.Barbers;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class BarberoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Barbero>().ToTable("Barberos");
            EntidadBaseConfiguracionBD<Barbero>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Barbero>().Property(e => e.UsuarioId).IsRequired();
            modelBuilder.Entity<Barbero>().Property(e => e.EstaAfiliadoABarberia).IsRequired().HasDefaultValue(false);
            modelBuilder.Entity<Barbero>().Property(e => e.BarberiaId);
            modelBuilder.Entity<Barbero>().Property(e => e.Direccion).IsRequired();
            modelBuilder.Entity<Barbero>().Property(e => e.SuscripcionId).IsRequired();
            modelBuilder.Entity<Barbero>().Property(e => e.HorarioApertura).IsRequired().HasDefaultValue("9:00");
            modelBuilder.Entity<Barbero>().Property(e => e.HorarioCierre).IsRequired().HasDefaultValue("18:00");
            modelBuilder.Entity<Barbero>().Property(e => e.EstadoSuscripcion).IsRequired().HasDefaultValue(EstadoSuscripcion.Pendiente);

            modelBuilder.Entity<Barbero>().HasOne(e => e.Suscripcion).WithMany(e => e.Barberos).HasForeignKey(e => e.SuscripcionId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Barbero>().HasOne(e => e.Barberia).WithMany(e => e.Barberos).HasForeignKey(e => e.BarberiaId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Barbero>()
            .HasOne(b => b.Usuario)               // Barberia tiene un Usuario
            .WithOne(u => u.Barbero)             // Usuario tiene una Barberia (relación 1 a 1)
            .HasForeignKey<Barbero>(b => b.UsuarioId)  // Clave foránea en Barberia
            .OnDelete(DeleteBehavior.Restrict);   // Evita borrado en cascada
        }
    }
}
