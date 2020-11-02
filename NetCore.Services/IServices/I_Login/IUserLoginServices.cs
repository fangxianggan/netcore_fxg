using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.DTO.ViewModel;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_Login
{
    public interface IUserLoginServices
    {
        Task<HttpReponseObjViewModel<ComplexTokenViewModel>> GetValidateLogon(UserLoginViewModel model);

        HttpReponseObjViewModel<ResBaseUserInfoViewModel> GetUserInfoData();

        HttpReponseViewModel GetLogout(string token);

        HttpReponseObjViewModel<TokenViewModel> GetRefreshTokenData(string refreshToken);


       

    }
}
