using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.Login
{
   public  class UserInfoViewModel
    {
        public string ID { set; get; }
        public string UserCode { set; get; }
        public string UserName { set; get; }

        public string Avatar { set; get; }

        public List<string> Roles { set; get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public DateTime RefreshExpires { set; get; }

    }

   
}
