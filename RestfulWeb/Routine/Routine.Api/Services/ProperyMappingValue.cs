using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    /// <summary>
    /// 映射 传参的目标属性和是否反转的flag
    /// </summary>
    public class ProperyMappingValue
    {
        public IEnumerable<string> DestionProperties { get; set; }

        public bool Revert { get; set; }
        public ProperyMappingValue(IEnumerable<string> destionProperties, bool revert=false)
        {
            this.DestionProperties = destionProperties??throw new ArgumentNullException(nameof(destionProperties));
            this.Revert = revert;
        }
    }
}
