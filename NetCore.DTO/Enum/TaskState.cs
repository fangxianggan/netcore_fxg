using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.Enum
{
    public  enum   TaskState
    {
        /// <summary>
        /// 未启动
        /// </summary>
        Init=0,
        /// <summary>
        /// 启动
        /// </summary>
        Start=1,
        /// <summary>
        /// 暂停
        /// </summary>
        Stop=2,
        /// <summary>
        /// 结束
        /// </summary>
        Finsh=3,
        /// <summary>
        /// 异常
        /// </summary>
        Exception=4
    }
}
