using API.Data.Entidades.Barbers;
using API.Data.Entidades.Nomencladores;
using API.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class ServicioConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Servicio>().ToTable("Servicios");
            EntidadBaseConfiguracionBD<Servicio>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Servicio>().Property(e => e.Nombre).IsRequired();
            modelBuilder.Entity<Servicio>().Property(e => e.Descripcion).IsRequired();
            modelBuilder.Entity<Servicio>().Property(e => e.Precio).IsRequired().HasPrecision(18, 2);
            modelBuilder.Entity<Servicio>().Property(e => e.TiempoDemora).IsRequired();
        }
    }
}
