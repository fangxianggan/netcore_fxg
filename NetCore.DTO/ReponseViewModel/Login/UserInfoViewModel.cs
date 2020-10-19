using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.Login
{
   public  class UserInfoViewModel
    {
        public string UserCode { set; get; }
        public string UserName { set; get; }

        public string Avatar { set; get; }

        public List<string> Roles { set; get; }

    }
}
