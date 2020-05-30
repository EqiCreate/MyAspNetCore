using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public  static class IEnumerableExtension
    {
        
        ///塑形，只返回需要的字段
        public static IEnumerable<ExpandoObject> ShapeData<Tsource>(this IEnumerable<Tsource> source, string fields)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var expandObj = new List<ExpandoObject>(source.Count());
            var properyInfoList = new List<PropertyInfo>();
            if (string.IsNullOrWhiteSpace(fields)) properyInfoList.AddRange(typeof(Tsource).GetProperties(BindingFlags.Public | BindingFlags.Instance));
            else
            {
                var fielsSplit = fields.Split(",");
                foreach (var field in fielsSplit)
                {
                    string properyName = field.Trim();
                    var propertyInfo = typeof(Tsource).GetProperty(properyName,BindingFlags.IgnoreCase| BindingFlags.Public | BindingFlags.Instance);
                    if(propertyInfo==null) throw new Exception($"无法找到{properyName}属性，类型为{typeof(Tsource)}");
                    properyInfoList.Add(propertyInfo);
                }
            }
            foreach (Tsource obj in source)
            {
                var shapeObj = new ExpandoObject();
                foreach (var propertyInfo in properyInfoList)
                {
                    shapeObj.TryAdd(propertyInfo.Name, propertyInfo.GetValue(obj));
                }
                expandObj.Add(shapeObj);
            }
            return expandObj;
        }
    }
}
