using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AddHeaderResultFilterAttribute : ResultFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public AddHeaderResultFilterAttribute(string name, string value)
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
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Add(
                _name, new string[] { _value });
            return base.OnResultExecutionAsync(context, next);
        }
    }
}
