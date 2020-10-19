using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.Enum
{
   public enum TokenType
    {
        /// <summary>
        /// 请求资源token
        /// </summary>
        AccessToken = 1,

        /// <summary>
        /// 刷新的token
        /// </summary>
        RefreshToken = 2
    }
}
