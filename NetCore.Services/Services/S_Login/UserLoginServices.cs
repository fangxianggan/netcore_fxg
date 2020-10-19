using Microsoft.AspNetCore.Http;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.DTO.ViewModel;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_JWTToken;
using NetCore.Services.IServices.I_Login;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_Login
{
    public class UserLoginServices : BaseServices<UserInfo, UserInfoViewModel>, IUserLoginServices
    {
        private readonly IBaseDomain<UserInfo> _baseDomain;
        private readonly IJWTTokenServices _jWTTokenServices;
        public UserLoginServices(IBaseDomain<UserInfo> baseDomain, IJWTTokenServices jWTTokenServices) : base(baseDomain)
        {
            _baseDomain = baseDomain;
            _jWTTokenServices = jWTTokenServices;
        }
        /// <summary>
        /// 验证登录 jwt
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpReponseViewModel<ComplexTokenViewModel>> GetValidateLogon(UserLoginViewModel model)
        {
            HttpReponseViewModel<ComplexTokenViewModel> httpReponse = new HttpReponseViewModel<ComplexTokenViewModel>();
            var ent = await _baseDomain.GetEntity(p => p.UserCode == model.UserCode);
            if (ent != null)
            {
                var password = DEncryptUtil.Md5Encrypt(model.Password);
                if (ent.Password.Equals(password))
                {
                    //生成 token
                    UserInfoViewModel u = ent.MapTo<UserInfoViewModel>();
                    ComplexTokenViewModel t = _jWTTokenServices.CreateToken(u);
                    httpReponse.Data = t;
                    httpReponse.ResultSign = ResultSign.Success;
                    httpReponse.Message = "登录成功";
                }
                else
                {
                    httpReponse.Message = "密码错误";
                    httpReponse.ResultSign = ResultSign.Error;
                    httpReponse.Data = null;
                }
            }
            else
            {
                httpReponse.Message = "用户不存在";
                httpReponse.ResultSign = ResultSign.Error;
                httpReponse.Data = null;
            }
            return httpReponse;
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public async Task<HttpReponseViewModel<UserInfoViewModel>> GetUserInfoData(TokenViewModel tokenModel)
        {
           
            return new HttpReponseViewModel<UserInfoViewModel>()
            {
                Code = 200,
                RequestParams = tokenModel,
                Flag = true,
                Message = HttpReponseMessageViewModel.SuccessMsg,
                ResultSign = ResultSign.Success,
                EXESql = "",
                Token = "",
                Data = new UserInfoViewModel()
                {
                    UserName = "yxd",
                    Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    Roles = new List<string>() { "admin" }
                }
            };
        }


        public HttpReponseViewModel<string> GetLogout(string token)
        {
            return new HttpReponseViewModel<string>()
            {
                Code = 200,
                RequestParams = token,
                Flag = true,
                Message = HttpReponseMessageViewModel.SuccessMsg,
                ResultSign = ResultSign.Success,
                EXESql = "",
                Token = "123123123",
                Data = "退出成功！"
            };
        }




    }
}
