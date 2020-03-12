using NNSoft.PL.Api;
using NNSoft.PL.Common;

namespace NNSoft.PL.Mappings
{
    class Mapper : IMapper
    {
        AutoMapper.IMapper mapper = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NativeServiceInfo, ServiceInfo>()
            .ForMember(
                dest => dest.State,
                opt => opt.MapFrom(src => (Common.ServiceState)src.State))
            .ReverseMap();
        }).CreateMapper();

        public T Map<T>(object source)
        {
            return mapper.Map<T>(source);
        }
    }
}
