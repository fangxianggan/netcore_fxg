using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.TaskJob
{
   public class HttpResultViewModel
    { /// <summary>
      /// 请求是否成功
      /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
