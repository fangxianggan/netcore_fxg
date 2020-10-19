using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Services.IServices.I_JWTToken;
using NetCore.DTO.ReponseViewModel.Login;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// token 校验
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private readonly IJWTTokenServices _jWTTokenServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jWTTokenServices"></param>
        public TokenController(IJWTTokenServices jWTTokenServices)
        {
            _jWTTokenServices = jWTTokenServices;
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
           var t=  _jWTTokenServices.CreateToken(u);
            return "Bearer " + t.AccessToken.TokenContent;
        }

    }
}