using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, ProperyMappingValue> _companyPropertyMapping = new Dictionary<string, ProperyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id",new ProperyMappingValue(new List<string>{ "Id"})},
            { "CompanyName",new ProperyMappingValue(new List<string>{ "Name"})},
            { "Country",new ProperyMappingValue(new List<string>{ "Country"})},
            { "Industry",new ProperyMappingValue(new List<string>{ "Industry"})},
            { "Product",new ProperyMappingValue(new List<string>{ "Product"})},
            { "Introduction",new ProperyMappingValue(new List<string>{ "Introduction"}) }
        };

        private readonly Dictionary<string, ProperyMappingValue> _employeePropertyMapping = new Dictionary<string, ProperyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id",new ProperyMappingValue(new List<string>{ "Id"})},
            { "CompanyId",new ProperyMappingValue(new List<string>{ "CompanyId"})},
            { "EmployeeNo",new ProperyMappingValue(new List<string>{ "EmployeeNo"})},
            { "Name",new ProperyMappingValue(new List<string>{ "FirstName","LastName"})},
            { "GenderDisplay",new ProperyMappingValue(new List<string>{ "Gender"})},
            { "Age",new ProperyMappingValue(new List<string>{ "DateofBirth"},true)},

        };
        private readonly IList<IPropertyMapping> propertyMapping = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            this.propertyMapping.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
            this.propertyMapping.Add(new PropertyMapping<CompanyDto, Company>(_companyPropertyMapping));
        }
        public Dictionary<string, ProperyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var machingmapping = this.propertyMapping.OfType<PropertyMapping<TSource, TDestination>>();
            if (machingmapping.Count() == 1) return machingmapping.First().MappingDictionary;
            throw new Exception("无法找到唯一映射关系" + typeof(TSource) + ":" + typeof(TDestination));
        }
        public bool ValidMappingExitFor<TSource, TDestination>(string fields)
        {
            var mapping = this.GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields)) return true;
            var fielsSplit = fields.Split(",");
            foreach (var field in fielsSplit)
            {
                var trimmedword = field.Trim();
                var indexofFirstSpace = trimmedword.IndexOf(" ");
                string properyName = indexofFirstSpace == -1 ? trimmedword : trimmedword.Remove(indexofFirstSpace);
                if (!mapping.ContainsKey(properyName)) return false;
            }
            return true;
        }

        
    }
}
