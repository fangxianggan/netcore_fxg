using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.Enum
{
    public enum UserState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Active = 0,
        /// <summary>
        /// 冻结
        /// </summary>
        Frozen = 1,
        /// <summary>
        /// 注销
        /// </summary>
        Cancel = 2

    }
}
