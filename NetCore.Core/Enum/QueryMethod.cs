using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.Enum
{
    /// <summary>
    /// SQL操作符
    /// </summary>
    public enum QueryMethod
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 1,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 2,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 3,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual = 4,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual = 5,
        /// <summary>
        /// 包含%-%
        /// </summary>
        Contains = 6,


        /// <summary>
        /// 开始包含-%
        /// </summary>
        StartsWith = 7,
        /// <summary>
        /// 结束包含%-
        /// </summary>
        EndsWith = 8,
        /// <summary>
        /// IN
        /// </summary>
        In = 9,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 10,

        /// <summary>
        /// not in 
        /// </summary>
        NotIn = 11,
        /// <summary>
        /// 不包含
        /// </summary>
        NotContains = 12,


        Null = 13,

        NotNull = 14,

        //不以...开始
        NotStartsWith = 15,

        //不结束于
        NotEndsWith = 16,

        Time = 17,
        //数组
        StdIn = 18,
        /// <summary>
        /// 在什么之间
        /// </summary>
        Between = 19,
        /// <summary>
        /// 处理时间段
        /// </summary>
        BetweenTime = 20,
    }
}
