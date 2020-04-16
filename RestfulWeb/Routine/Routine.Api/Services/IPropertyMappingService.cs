using System.Collections.Generic;

namespace Routine.Api.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, ProperyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExitFor<TSource, TDestination>(string fields);
    }
}