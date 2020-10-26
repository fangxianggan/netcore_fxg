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
using NetCore.Services.IServices.I_Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_Login
{
    public class UserLoginServices : BaseServices<UserInfo, UserInfoViewModel>, IUserLoginServices
    {
        private readonly IBaseDomain<UserInfo> _baseDomain;
        private readonly IJWTTokenServices _jWTTokenServices;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRedisServices _redisService;
        public UserLoginServices(IRedisServices redisService, IBaseDomain<UserInfo> baseDomain, IJWTTokenServices jWTTokenServices, IHttpContextAccessor contextAccessor) : base(baseDomain)
        {
            _baseDomain = baseDomain;
            _jWTTokenServices = jWTTokenServices;
            _contextAccessor = contextAccessor;
            _redisService = redisService;
        }
        /// <summary>
        /// 验证登录 jwt
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpReponseObjViewModel<ComplexTokenViewModel>> GetValidateLogon(UserLoginViewModel model)
        {
            HttpReponseObjViewModel<ComplexTokenViewModel> httpReponse = new HttpReponseObjViewModel<ComplexTokenViewModel>();
            var ent = await _baseDomain.GetEntity(p => p.UserName == model.UserName);
            if (ent != null)
            {
                var password = DEncryptUtil.Md5Encrypt(model.Password);
                if (ent.Password.Equals(password))
                {
                    //生成 token
                    UserInfoViewModel u = ent.MapTo<UserInfoViewModel>();
                    u.Roles = new List<string>() { "abc", "nnn" };
                    ComplexTokenViewModel t = _jWTTokenServices.CreateToken(u);

                    //key
                    string key = t.RefreshToken.TokenContent;
                    if (_redisService.GetIsExistKey(key))
                    {
                        //删除
                        _redisService.RemoveRefreshTokenValue(key);

                        
                    }
                    //更新 过期的时间
                    u.Expires = t.AccessToken.Expires;
                    u.RefreshExpires = t.RefreshToken.Expires;
                    //存储刷新的token  存入redis
                    string value = JsonUtil.JsonSerialize(u);
                    _redisService.SetRefreshTokenValue(key, value);

                    httpReponse.Data = t;
                    httpReponse.ResultSign = ResultSign.Success;
                    httpReponse.Message = "登录成功";
                }
                else
                {
                    httpReponse.Message = "密码错误";
                    httpReponse.ResultSign = ResultSign.Error;
                    httpReponse.Data = new ComplexTokenViewModel();
                    httpReponse.StatusCode = StatusCode.PasswordError;
                }
            }
            else
            {
                httpReponse.Message = "用户不存在";
                httpReponse.ResultSign = ResultSign.Error;
                httpReponse.Data = new ComplexTokenViewModel();
                httpReponse.StatusCode = StatusCode.UserNotExist;
            }
            return httpReponse;
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public async Task<HttpReponseObjViewModel<UserInfoViewModel>> GetUserInfoData(string tokenModel)
        {
            var user = _contextAccessor.HttpContext.User ;


            return new HttpReponseObjViewModel<UserInfoViewModel>()
            {
                StatusCode = StatusCode.OK,
                Message = HttpReponseMessage.SuccessMsg,
                ResultSign = ResultSign.Success,
                Data = new UserInfoViewModel()
                {
                    UserName = "yxd",
                    Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    Roles = new List<string>() { "admin" }
                }
            };
        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HttpReponseViewModel GetLogout(string token)
        {
            //清除 redis 用户token数据
            string key = token;
            if (_redisService.GetIsExistKey(key))
            {
                //删除
                _redisService.RemoveRefreshTokenValue(key);
            }
            return new HttpReponseViewModel();
        }

        /// <summary>
        /// 刷新 返回新的token 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public HttpReponseObjViewModel<TokenViewModel> GetRefreshTokenData(string refreshToken)
        {
            HttpReponseObjViewModel<TokenViewModel> httpReponse = new HttpReponseObjViewModel<TokenViewModel>();
            var str = _redisService.GetValueFromHash(refreshToken);
            UserInfoViewModel userInfoView = JsonUtil.JsonDeserializeObject<UserInfoViewModel>(str);
            var dt = DateTime.Now;
            if (userInfoView.Expires < dt && dt < userInfoView.RefreshExpires)
            {
                var  t= _jWTTokenServices.RefreshToken(userInfoView);
                httpReponse.Data = t;

                string key = refreshToken;
                if (_redisService.GetIsExistKey(key))
                {
                    //删除
                    _redisService.RemoveRefreshTokenValue(key);
                }
                //更新 过期的时间
                userInfoView.Expires = t.Expires;
                //存储刷新的token  存入redis
                string value = JsonUtil.JsonSerialize(userInfoView);
                _redisService.SetRefreshTokenValue(key, value);
            }
            else
            {
                httpReponse.Data = new TokenViewModel();
                httpReponse.StatusCode = StatusCode.RefreshTokenError;
                httpReponse.Message = "刷新token发生错误";
                httpReponse.ResultSign = ResultSign.Error;
            }
            return httpReponse;
        }

    }
}
