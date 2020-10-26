using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.EntityModel.ReponseModels
{
    public class HttpReponseMessage
    {
        public static string SuccessMsg { get { return "操作成功!"; } }

        public static string ErrorMsg { get { return "操作失败!"; } }

        public static string HaveDelete { get { return "已经删除!"; } }
    }
}
