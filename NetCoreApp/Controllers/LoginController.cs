using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.Services.IServices;
using NetCore.Services.IServices.I_Login;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IUserLoginServices _userLoginServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLoginServices"></param>
        public LoginController(IUserLoginServices userLoginServices)
        {
            _userLoginServices = userLoginServices;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost,Route("GetValidateLogon")]
        public async Task<HttpReponseViewModel<string>> GetValidateLogon(UserLoginViewModel model)
        {
            return await Task.Run(() =>
            {
                return _userLoginServices.GetValidateLogon(model);
            });
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetUserInfoData")]
        public async Task<HttpReponseViewModel<UserInfoViewModel>> GetUserInfoData(string token)
        {
            return await Task.Run(() =>
            {
                return _userLoginServices.GetUserInfoData(token);
            });
        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetLogout")]
        public async Task<HttpReponseViewModel<string>> GetLogout(string token)
        {
            return await Task.Run(() =>
            {
                return _userLoginServices.GetLogout(token);
            });
        }

    }
}