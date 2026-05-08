using API.Data.Entidades.Barbers;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class ResenhaConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resenha>().ToTable("Resenhas");
            EntidadBaseConfiguracionBD<Resenha>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Resenha>().Property(e => e.UsuarioId).IsRequired();
            modelBuilder.Entity<Resenha>().Property(e => e.Fecha).IsRequired();

            modelBuilder.Entity<Resenha>().HasOne(e => e.Usuario).WithMany(e => e.Resenhas).HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
