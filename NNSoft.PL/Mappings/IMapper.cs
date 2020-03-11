using System;

namespace NNSoft.PL.Mappings
{
    public interface IMapper
    {
        T Map<T>(object source);
    }
}
