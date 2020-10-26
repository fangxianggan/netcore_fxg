using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetCore.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="modelMetadataProvider"></param>
        public CustomerExceptionFilter(IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }
        /// <summary>
        /// 发生异常进入
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)//如果异常没有处理
            {
                if (_hostingEnvironment.IsDevelopment())//如果是开发环境
                {

                    LogUtil.Error($"错误日志:{context.Exception.Message}");
                    //var result = new ViewResult { ViewName = "../Handle/Index" };
                    //result.ViewData = new ViewDataDictionary(_modelMetadataProvider,
                    //                                            context.ModelState);
                    //result.ViewData.Add("Exception", context.Exception);//传递数据
                    //context.Result = result;
                }
                else
                {
                    context.Result = new JsonResult(new
                    {
                        Result = false,
                        Code = 500,
                        Message = context.Exception.Message
                    });
                }
                context.ExceptionHandled = true;//异常已处理
            }

            base.OnException(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task OnExceptionAsync(ExceptionContext context)
        {
           return  base.OnExceptionAsync(context);
        }
    }
}
