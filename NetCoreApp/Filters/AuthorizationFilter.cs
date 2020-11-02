using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
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
    public class AuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            //验证
            if (!context.Filters.Any(item => item is AllowAnonymousFilter))
            {
                IIdentity user = context.HttpContext.User.Identity;
                if (!user.IsAuthenticated)
                {
                    //返回401未授权
                    context.Result = new JsonResult(new HttpReponseViewModel()
                    {
                        ExeSql = "",
                        Message = "未授权",
                        ResultSign = ResultSign.Error,
                        StatusCode = StatusCode.Unauthorized
                    });
                }
            }
            await Task.CompletedTask;
            ////根据当前用户，判断当前访问的action，没有权限时返回403错误
            //context.Result = new ForbidResult();

            //return Task.CompletedTask;

            // await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
        }
    }
}
