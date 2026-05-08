using API.Data.Entidades.Barbers;
using API.Data.Entidades.Nomencladores;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class ProductoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().ToTable("Productos");
            EntidadBaseConfiguracionBD<Producto>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Producto>().Property(e => e.Nombre).IsRequired();
            modelBuilder.Entity<Producto>().Property(e => e.Descripcion).IsRequired();
            modelBuilder.Entity<Producto>().Property(e => e.Precio).IsRequired().HasPrecision(18, 2);
        }
    }
}
