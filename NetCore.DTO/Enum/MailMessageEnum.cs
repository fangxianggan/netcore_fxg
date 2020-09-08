using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.Enum
{
    public enum MailMessageEnum
    {
        /// <summary>
        /// 不通知
        /// </summary>
        None = 0,
        /// <summary>
        /// 异常通知
        /// </summary>
        Err = 1,
        /// <summary>
        /// 全部通知
        /// </summary>
        All = 2
    }
}
