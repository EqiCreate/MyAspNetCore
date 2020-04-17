using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public static class ObjectExtension
    {
        public static ExpandoObject ShapeData<Tsource>(this Tsource source, string fields)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var expandObj = new ExpandoObject();
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(Tsource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in propertyInfos)
                {
                    expandObj.TryAdd(property.Name,property.GetValue(source));
                }

            }
            else
            {
                var fielsSplit = fields.Split(",");
                foreach (var field in fielsSplit)
                {
                    string properyName = field.Trim();
                    var propertyInfo = typeof(Tsource).GetProperty(properyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null) throw new Exception($"无法找到{properyName}属性，类型为{typeof(Tsource)}");
                    expandObj.TryAdd(propertyInfo.Name, propertyInfo.GetValue(source));
                }
            }
            return expandObj;
        }
    }
}
