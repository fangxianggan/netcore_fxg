using Microsoft.AspNetCore.Mvc;
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
    public class CacheResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {
        private static readonly Dictionary<string, object> _Cache = new Dictionary<string, object>();
        private string _cacheKey;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();
            if (_Cache.ContainsKey(_cacheKey))
            {
                var cachedValue = _Cache[_cacheKey] as ViewResult;
                if (cachedValue != null)
                {
                    context.Result = cachedValue; //设置该Result将是filter管道短路，阻止执行管道的其他阶段
                }
            }

            await next();

            if (!String.IsNullOrEmpty(_cacheKey) &&
               !_Cache.ContainsKey(_cacheKey))
            {
                var objectResult = (ObjectResult)context.Result;
                if (objectResult!=null)
                {
                    _Cache.Add(_cacheKey, "mmmmmmmmmm");
                }
            }

        }
    }
}
