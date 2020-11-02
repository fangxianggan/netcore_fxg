using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace NetCoreApp.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionFilter : Attribute, IAsyncActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //  await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");

            var stopwachKey = Guid.NewGuid();
            //开始请求数据时间
            var stopwach = new Stopwatch();
            stopwach.Start();
            context.HttpContext.Items.Add(stopwachKey, stopwach);

            //模型验证
            if (context.ModelState.IsValid)
            {
                await next();
            }
            else
            {
                var modelStateList = context.ModelState.ToList();
                string errorMsg = "";
                foreach (var item in modelStateList)
                {
                    errorMsg += item.Value.Errors.First().ErrorMessage+ "</br>";
                }
                //模型验证失败 跳转出错的filter
                //var modelState = context.ModelState.FirstOrDefault(f => f.Value.Errors.Any());
                //string errorMsg = modelState.Value.Errors.First().ErrorMessage;
                //返回错误
                context.Result = new JsonResult(new HttpReponseViewModel()
                {
                    ExeSql = "",
                    Message = "模型验证失败 :</br> " + errorMsg,
                    ResultSign = ResultSign.Error,
                    StatusCode = StatusCode.ModelStateError
                });

            }

            //请求结束时间
            //  await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
            stopwach.Stop();
            var time = stopwach.Elapsed;
            //超出5秒 生成警告日志
            if (time.TotalSeconds > 5)
            {
                LogUtil.Warn($"{context.ActionDescriptor.DisplayName}执行耗时:{time.ToString()}");
            }
        }
    }
}
