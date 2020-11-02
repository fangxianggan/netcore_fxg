using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.RequestViewModel.Login
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class UserLogonViewModel
    {
        public Guid ID { set; get; }
        public string UserCode { set; get; }

        public string UserName { set; get; }

        public string Password { set; get; }

        public int Gender { set; get; }

        public string PhoneNumber { set; get; }

        public string Email { set; get; }
    }
}
