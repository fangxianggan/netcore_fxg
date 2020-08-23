using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.Enum
{
    public enum ResultSign
    {
        /// <summary>
        ///     操作成功(增删改  成功表示可以操作)
        /// </summary>
        Successful = 0,

        /// <summary>
        ///     警告(不可删除 不可修改,未授权 ，超时等)
        /// </summary>
        Warning = 1,

        /// <summary>
        ///     操作引发错误（操作报错引起的事件）
        /// </summary>
        Error = 2,

        /// <summary>
        /// 消息 （查询操作)
        /// </summary>
        Info = 3
    }
}
