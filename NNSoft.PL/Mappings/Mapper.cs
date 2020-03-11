using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSoft.PL.Mappings
{
    class Mapper : IMapper
    {
        AutoMapper.IMapper mapper = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Api.ServiceInfo, Common.ServiceInfo>()
            .ReverseMap();
        }).CreateMapper();

        public T Map<T>(object source)
        {
            return mapper.Map<T>(source);
        }
    }
}
