using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class DbAttribute : Attribute
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Name { get; set; }
    }
}
