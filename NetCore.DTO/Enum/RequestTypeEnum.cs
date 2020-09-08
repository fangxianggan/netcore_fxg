using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.Enum
{
    public enum RequestTypeEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// get 方式
        /// </summary>
        Get = 1,
        /// <summary>
        ///  post 方式
        /// </summary>
        Post = 2,
        /// <summary>
        /// put 方式
        /// </summary>
        Put = 4,
        /// <summary>
        /// delete 方式
        /// </summary>
        Delete = 8
    }
}
