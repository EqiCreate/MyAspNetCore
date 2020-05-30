using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public static class IQueryableExtension
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="mappingdict"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplySource<T>(this IQueryable<T> source, string orderBy, Dictionary<string, ProperyMappingValue> mappingdict)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (mappingdict == null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(orderBy)) return source;
            var orderByAfterSplit = orderBy.Split(",");
            foreach (var orderByUnit in orderByAfterSplit.Reverse())//取反
            {
                var trimmedword = orderByUnit.Trim();
                var orderDesc = trimmedword.EndsWith("desc");
                var indexofFirstSpace = trimmedword.IndexOf(" ");
                string properyName = indexofFirstSpace == -1 ? trimmedword : trimmedword.Remove(indexofFirstSpace);
                if (!mappingdict.ContainsKey(properyName)) throw new ArgumentNullException($"无法找到对应{properyName}映射");
                var properymappingvalue = mappingdict[properyName];
                if (properymappingvalue==null) throw new ArgumentNullException(nameof(properymappingvalue));
                if (properymappingvalue.Revert) orderDesc = !orderDesc;
                foreach (var mappingValue in properymappingvalue.DestionProperties.Reverse())
                {
                    source = source.OrderBy(mappingValue + (orderDesc? " descending":" ascending"));
                }
            }
            return source;
        }
    }
}
