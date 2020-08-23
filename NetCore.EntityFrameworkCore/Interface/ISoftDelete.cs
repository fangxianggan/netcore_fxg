using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetCore.EntityFrameworkCore.Interface
{
    public interface ISoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        /// 
        [DisplayName("是否删除")]
        bool IsDelete { set; get; }
    }
}
