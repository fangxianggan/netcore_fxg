using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public class RabbitLoggerProvider : ILoggerProvider,IDisposable
    {
        RabbitMQLoggerOptions loggerOptions;

        public RabbitLoggerProvider(IOptionsMonitor<RabbitMQLoggerOptions> options)
        {
            loggerOptions = options.CurrentValue;
        }

        /// <summary>
        /// 创建Logger对象
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            //可缓存实例，这里略过了
            return new RabbitLogger(categoryName, loggerOptions);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
