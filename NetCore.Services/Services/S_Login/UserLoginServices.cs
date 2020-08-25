using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.RequestViewModel.Login;
using NetCore.Services.IServices;
using NetCore.Services.IServices.I_Login;
using System.Collections.Generic;

namespace NetCore.Services.Services.S_Login
{
    public class UserLoginServices : IUserLoginServices
    {
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpReponseViewModel<string> GetValidateLogon(UserLoginViewModel model)
        {

            return new HttpReponseViewModel<string>()
            {
                Code = 20000,
                RequestParams = model,
                Flag = true,
                Message = HttpReponseMessageViewModel.SuccessMsg,
                ResultSign = ResultSign.Successful,
                EXESql = "",
                Token = "123123123",
                Data = "登录成功！"
            };
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HttpReponseViewModel<UserInfoViewModel> GetUserInfoData(string token)
        {
            return new HttpReponseViewModel<UserInfoViewModel>()
            {
                Code = 20000,
                RequestParams = token,
                Flag = true,
                Message = HttpReponseMessageViewModel.SuccessMsg,
                ResultSign = ResultSign.Successful,
                EXESql = "",
                Token = "",
                Data = new UserInfoViewModel() {
                     Name="yxd",
                     Avatar= "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                     Roles=new List<string>() { "admin"}
                }
            };
        }


        public HttpReponseViewModel<string> GetLogout(string token)
        {
            return new HttpReponseViewModel<string>()
            {
                Code = 20000,
                RequestParams = token,
                Flag = true,
                Message = HttpReponseMessageViewModel.SuccessMsg,
                ResultSign = ResultSign.Successful,
                EXESql = "",
                Token = "123123123",
                Data = "退出成功！"
            };
        }

       

      
    }
}
