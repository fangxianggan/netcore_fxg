using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_Login
{
    public class UserLogonServices : BaseServices<UserInfo, UserLogonViewModel>, IUserLogonServices
    {
        private readonly IBaseDomain<UserInfo> _baseDomain;
        public UserLogonServices(IBaseDomain<UserInfo> baseDomain) : base(baseDomain)
        {
            _baseDomain = baseDomain;
        }

        public async Task<HttpReponseObjViewModel<string>> GetUserLogonData(UserLogonViewModel model)
        {
            model.Password = DEncryptUtil.Md5Encrypt(model.Password);
            return  await AddOrEditService(model);
        }

       
    }
}
