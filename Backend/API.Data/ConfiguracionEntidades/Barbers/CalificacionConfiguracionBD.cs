using API.Data.Entidades.Barbers;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class CalificacionConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calificacion>().ToTable("Calificaciones");
            EntidadBaseConfiguracionBD<Calificacion>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Calificacion>().Property(e => e.UsuarioId).IsRequired();
            modelBuilder.Entity<Calificacion>().Property(e => e.CantidadEstrellas).IsRequired();

            modelBuilder.Entity<Calificacion>().HasOne(e => e.Usuario).WithMany(e => e.Calificaciones).HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
