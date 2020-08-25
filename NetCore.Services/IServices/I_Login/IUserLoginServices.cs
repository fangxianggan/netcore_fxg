using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel.Login;

namespace NetCore.Services.IServices.I_Login
{
    public interface IUserLoginServices
    {
         HttpReponseViewModel<string> GetValidateLogon(UserLoginViewModel model);

         HttpReponseViewModel<UserInfoViewModel> GetUserInfoData(string token);

         HttpReponseViewModel<string> GetLogout(string token);

    }
}
