using API.Data.Entidades.Barbers;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Barbers
{
    public class HistoricoSuscripcionBarberoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().ToTable("HistoricosSuscripcionesBarberos");
            EntidadBaseConfiguracionBD<HistoricoSuscripcionBarbero>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.BarberoId).IsRequired();
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.TipoSuscripcion).IsRequired();
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.NivelSuscripcion).IsRequired();
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.Nombre).IsRequired();
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.Descripcion).IsRequired();
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.Precio).IsRequired();
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.TiempoVigencia).IsRequired();
            modelBuilder.Entity<HistoricoSuscripcionBarbero>().Property(e => e.EsFree).IsRequired().HasDefaultValue(false);
        }
    }
}
