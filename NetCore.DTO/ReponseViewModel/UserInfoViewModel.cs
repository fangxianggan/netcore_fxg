using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel
{
   public  class UserInfoViewModel
    {
        public string Name { set; get; }

        public string Avatar { set; get; }

        public List<string> Roles { set; get; }

    }
}
