using API.Data.Entidades.Barbers;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class ReservaConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reserva>().ToTable("Reservas");
            EntidadBaseConfiguracionBD<Reserva>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Reserva>().Property(e => e.ClienteId).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.Fecha).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.Hora).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.ServicioId).IsRequired();
            modelBuilder.Entity<Reserva>().Property(e => e.EstadoReserva).IsRequired().HasDefaultValue(EstadoReserva.Pendiente);
        }
    }
}
