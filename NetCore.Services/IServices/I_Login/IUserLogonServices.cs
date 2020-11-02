using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.RequestViewModel.Login;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_Login
{
    public interface IUserLogonServices
    {
        Task<HttpReponseObjViewModel<string>> GetUserLogonData(UserLogonViewModel model);
    }
}
