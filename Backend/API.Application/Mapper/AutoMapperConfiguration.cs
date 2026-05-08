using API.Application.Mapper.Barbers;
using API.Application.Mapper.Nomencladores;
using API.Application.Mapper.Seguridad;
using AutoMapper;

namespace API.Application.Mapper
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMappers(this IServiceCollection services, MapperConfigurationExpression cfg)
        {
            IMapper mapper = new MapperConfiguration(cfg).CreateMapper();
            services.AddSingleton(mapper);
        }
        public static MapperConfigurationExpression AddAutoMapperLeadOportunidade(this MapperConfigurationExpression cfg)
        {
            cfg.AddProfile<UsuarioDtoProfile>();
            cfg.AddProfile<RolDtoProfile>();
            cfg.AddProfile<PermisoDtoProfile>();

            cfg.AddProfile<CategoriaDtoProfile>();
            cfg.AddProfile<OrigenDtoProfile>();
            cfg.AddProfile<GrupoDtoProfile>();
            cfg.AddProfile<FamiliaDtoProfile>();

            cfg.AddProfile<BarberiaDtoProfile>();
            cfg.AddProfile<BarberoDtoProfile>();
            cfg.AddProfile<ClienteDtoProfile>();
            cfg.AddProfile<ComercialDtoProfile>();
            cfg.AddProfile<ProductoDtoProfile>();
            cfg.AddProfile<ReservaDtoProfile>();
            cfg.AddProfile<ServicioDtoProfile>();
            cfg.AddProfile<SuscripcionDtoProfile>();
            cfg.AddProfile<MonedaDtoProfile>();
            cfg.AddProfile<AdminDtoProfile>();
            cfg.AddProfile<ResenhaDtoProfile>();
            cfg.AddProfile<CalificacionDtoProfile>();

            return cfg;
        }
        public static MapperConfigurationExpression CreateExpression()
        {
            return new MapperConfigurationExpression();
        }
    }
}
