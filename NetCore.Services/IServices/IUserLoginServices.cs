using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.RequestViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.IServices
{
    public interface IUserLoginServices
    {
         HttpReponseViewModel<string> GetValidateLogon(UserLoginViewModel model);

         HttpReponseViewModel<UserInfoViewModel> GetUserInfoData(string token);

         HttpReponseViewModel<string> GetLogout(string token);

    }
}
