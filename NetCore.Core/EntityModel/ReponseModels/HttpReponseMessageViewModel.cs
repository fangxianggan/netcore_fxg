using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.EntityModel.ReponseModels
{
  public  class HttpReponseMessageViewModel
    {
        public static string SuccessMsg { get { return "数据操作成功!"; } }

        public static string ErrorMsg { get { return "数据操作失败!"; } }

        public static string HaveDelete { get { return "数据已经删除!"; } }
    }
}
