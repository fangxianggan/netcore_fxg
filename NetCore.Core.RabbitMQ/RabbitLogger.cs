using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public class RabbitLogger : ILogger, IDisposable
    {
        string category;
        RabbitMQLoggerOptions loggerOptions;
        RabbitMQProducer producer;

        public RabbitLogger(string category, RabbitMQLoggerOptions options)
        {
            this.category = category;
            this.loggerOptions = options;

            producer = new RabbitMQProducer(options.Hosts);
            producer.Password = options.Password;
            producer.UserName = options.UserName;
            producer.Port = options.Port;
            producer.VirtualHost = options.VirtualHost;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
        }
        /// <summary>
        /// 是否记录日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            //只记录日志等级大于指定最小等级且属于Rabbit分类的日志
            return logLevel >= loggerOptions.MinLevel && category.Contains(loggerOptions.Category, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                string message = "";
                if (state != null)
                {
                    message = state.ToString();
                }
                if (exception != null)
                {
                    message += Environment.NewLine + formatter?.Invoke(state, exception);
                }
                //发送消息
                producer.Publish(loggerOptions.Queue, message, options =>
                {
                    options.Arguments = loggerOptions.Arguments;
                    options.AutoDelete = loggerOptions.AutoDelete;
                    options.Durable = loggerOptions.Durable;
                });
            }
        }
    }
}
