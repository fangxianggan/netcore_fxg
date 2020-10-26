using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NetCoreApp.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            //IIdentity user = context.HttpContext.User.Identity;
            //if (!user.IsAuthenticated)
            //{
            //    var url = "/index";
            //    //跳转到登录页
            //    context.Result = new LocalRedirectResult(url);
            //    return Task.CompletedTask;
            //}

            ////根据当前用户，判断当前访问的action，没有权限时返回403错误
            //context.Result = new ForbidResult();

            //return Task.CompletedTask;

             await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
        }
    }
}
