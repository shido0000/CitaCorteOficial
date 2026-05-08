using API.Application.Dtos.Barbers.Suscripcion;
using API.Data.Entidades.Barbers;

namespace API.Application.Mapper.Barbers
{
    public class SuscripcionDtoProfile : BaseProfile<Suscripcion, SuscripcionDto, CrearSuscripcionInputDto, ActualizarSuscripcionInputDto, ListadoPaginadoSuscripcionDto>
    {
        public SuscripcionDtoProfile()
        {
            MapSuscripcionDto();
            MapDetallesSuscripcionDto();
            MapListadoSuscripcionDto();
        }

        public void MapSuscripcionDto()
        {
            CreateMap<Suscripcion, DetallesSuscripcionDto>()
                .ReverseMap();
        }

        public void MapDetallesSuscripcionDto()
        {
            CreateMap<Suscripcion, DetallesSuscripcionDto>()
            .ForMember(dto => dto.CodigoMoneda, opt => opt.MapFrom(e => e.Moneda.Codigo))
            .ForMember(dto => dto.DescripcionMoneda, opt => opt.MapFrom(e => e.Moneda.Descripcion))
            .ForMember(dto => dto.CaracteristicaSuscripcion, opt => opt.MapFrom(e =>
                    e.CaracteristicaSuscripciones.Select(c => $"{c.Descripcion}").ToList()
                ));
        }

        public void MapListadoSuscripcionDto()
        {
            CreateMap<Suscripcion, ListadoPaginadoSuscripcionDto>()
            .ForMember(dto => dto.CodigoMoneda, opt => opt.MapFrom(e => e.Moneda.Codigo))
            .ForMember(dto => dto.DescripcionMoneda, opt => opt.MapFrom(e => e.Moneda.Descripcion))
            .ForMember(dto => dto.CaracteristicaSuscripcion, opt => opt.MapFrom(e =>
                    e.CaracteristicaSuscripciones.Select(c => $"{c.Descripcion}").ToList()
                ));
        }
    }
}


