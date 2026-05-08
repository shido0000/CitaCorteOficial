using API.Data.Entidades.Barbers;
using API.Data.Entidades.Nomencladores;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class NotificacionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notificacion>().ToTable("Notificaciones");
            EntidadBaseConfiguracionBD<Notificacion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Notificacion>().Property(e => e.UsuarioId).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.Titulo).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.Mensaje).IsRequired();
            modelBuilder.Entity<Notificacion>().Property(e => e.FueLeido).IsRequired().HasDefaultValue(false);
        }
    }
}
