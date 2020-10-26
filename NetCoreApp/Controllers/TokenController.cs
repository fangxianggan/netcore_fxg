using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Services.IServices.I_JWTToken;
using NetCore.DTO.ReponseViewModel.Login;
using NetCore.DTO.ViewModel;
using System.Security.Claims;
using NetCore.Core.EntityModel.ReponseModels;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// token 校验
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private readonly IJWTTokenServices _jWTTokenServices;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jWTTokenServices"></param>
        /// <param name="contextAccessor"></param>
        public TokenController(IJWTTokenServices jWTTokenServices, IHttpContextAccessor contextAccessor)
        {
            _jWTTokenServices = jWTTokenServices;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("/GetTestToken")]
        [HttpGet]
        public string TestToken()
        {
            UserInfoViewModel u = new UserInfoViewModel() { UserCode = "admin", UserName = "admin" };
            var t = _jWTTokenServices.CreateToken(u);
            return "Bearer " + t.AccessToken.TokenContent;
        }

      
    }
}