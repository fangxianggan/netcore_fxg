using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.DTO.ViewModel;
using NetCore.Services.IServices;
using NetCore.Services.IServices.I_Login;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly IUserLoginServices _userLoginServices;
        private readonly IHttpContextAccessor _contextAccessor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLoginServices"></param>
        /// <param name="contextAccessor"></param>
        public LoginController(IUserLoginServices userLoginServices, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _userLoginServices = userLoginServices;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        [AllowAnonymous]
        [HttpPost, Route("GetValidateLogon")]
        public async Task<HttpReponseObjViewModel<ComplexTokenViewModel>> GetValidateLogon(UserLoginViewModel model)
        {
            return await Task.Run(() =>
            {
                return _userLoginServices.GetValidateLogon(model);
            });
        }

        /// <summary>
        /// 前端接口 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetUserInfoData")]
        public async Task<HttpReponseObjViewModel<ResBaseUserInfoViewModel>> GetUserInfoData()
        {
            return await Task.Run(() =>
            {
                return _userLoginServices.GetUserInfoData();
            });
        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetLogout")]
        public async Task<HttpReponseViewModel> GetLogout([FromBody] string token)
        {
            return await Task.Run(() =>
            {
                return _userLoginServices.GetLogout(token);
            });
        }


        /// <summary>
        /// 刷新新的token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("GetRefreshToken")]
        public async Task<HttpReponseObjViewModel<TokenViewModel>> GetRefreshToken(string refreshToken)
        {
            return await Task.Run(() =>
            {
                return _userLoginServices.GetRefreshTokenData(refreshToken);
            });
        }

    }
}