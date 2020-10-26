using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetCoreApp.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceFilter :Attribute,IAsyncResourceFilter
    {
        private readonly string[] _headers;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        public ResourceFilter(params string[] headers)
        {
            _headers = headers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
           // await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
            if (_headers == null) return;

            if (!_headers.All(m => context.HttpContext.Request.Headers.ContainsKey(m)))
            {
                context.Result = new JsonResult(new { Error = "Headers Missing" })
                { StatusCode = 400 };
                ;
                return;

                throw new NotImplementedException();
            }
            ResourceExecutedContext executedContext = await next();
           // await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        }
    }
}
