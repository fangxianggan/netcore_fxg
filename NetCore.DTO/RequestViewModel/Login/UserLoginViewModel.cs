using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.RequestViewModel.Login
{
   public  class UserLoginViewModel
    {
        public string UserCode { set; get; }
        public string UserName { set; get; }

        public string Password { set; get; }
    }
}
