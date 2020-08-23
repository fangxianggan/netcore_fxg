using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Attributes
{
    /// <summary>
    /// 表示一个特性,标识该特性的Action方法绕过认证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class IgnoreAttribute : Attribute
    {

    }
}
