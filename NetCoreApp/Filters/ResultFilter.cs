using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Filters
{
    /// <summary>
    /// result 加入head头
    /// </summary>
    public class ResultFilter :Attribute, IAsyncResultFilter
    {
        private readonly string _name;
        private readonly string _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ResultFilter(string name, string value)
        {
            _name = name;
            _value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
           // await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
            context.HttpContext.Response.Headers.Add(_name, new string[] { _value });
            if (!(context.Result is EmptyResult))
            {
                await next();
            }
            else
            {
                context.Cancel = true;
            }

          //  await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        }
    }
}
