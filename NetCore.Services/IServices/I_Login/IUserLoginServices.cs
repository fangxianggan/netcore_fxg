using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.DTO.ViewModel;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_Login
{
    public interface IUserLoginServices
    {
        Task<HttpReponseViewModel<ComplexTokenViewModel>> GetValidateLogon(UserLoginViewModel model);

        Task<HttpReponseViewModel<UserInfoViewModel>> GetUserInfoData(TokenViewModel tokenModel);

         HttpReponseViewModel<string> GetLogout(string token);

    }
}
