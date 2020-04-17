using System.Reflection;

namespace Routine.Api.Services
{
    public class PropertyCheckServices : IPropertyCheckServices
    {
        public bool TypeHasProPerty<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields)) return true;
            else
            {
                var fielsSplit = fields.Split(",");
                foreach (var field in fielsSplit)
                {
                    string properyName = field.Trim();
                    var propertyInfo = typeof(T).GetProperty(properyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null) return false;
                }
                return true;
            }
        }
    }
}
