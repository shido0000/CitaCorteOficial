using API.Data.Entidades.Barbers;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class SolicitudDeAfiliacionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolicitudDeAfiliacion>().ToTable("SolicitudDeAfiliaciones");
            EntidadBaseConfiguracionBD<SolicitudDeAfiliacion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<SolicitudDeAfiliacion>().Property(e => e.FechaSolicitado).IsRequired();
            modelBuilder.Entity<SolicitudDeAfiliacion>().Property(e => e.EstadoSolicitudAfiliacion).IsRequired().HasDefaultValue(EstadoSolicitudAfiliacion.Pendiente);

            modelBuilder.Entity<SolicitudDeAfiliacion>()
             .HasOne(b => b.Barbero)               // SolicitudDeAfiliacion tiene un Usuario
             .WithOne(u => u.SolicitudDeAfiliacion)             // Usuario tiene una SolicitudDeAfiliacion (relación 1 a 1)
             .HasForeignKey<SolicitudDeAfiliacion>(b => b.BarberoId)  // Clave foránea en SolicitudDeAfiliacion
             .OnDelete(DeleteBehavior.Restrict);   // Evita borrado en cascada

            modelBuilder.Entity<SolicitudDeAfiliacion>()
            .HasOne(b => b.Barberia)               // SolicitudDeAfiliacion tiene un Usuario
            .WithOne(u => u.SolicitudDeAfiliacion)             // Usuario tiene una SolicitudDeAfiliacion (relación 1 a 1)
            .HasForeignKey<SolicitudDeAfiliacion>(b => b.BarberiaId)  // Clave foránea en SolicitudDeAfiliacion
            .OnDelete(DeleteBehavior.Restrict);   // Evita borrado en cascada
        }
    }
}
