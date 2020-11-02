using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public class RabbitMQProducer : RabbitBase
    {
        public RabbitMQProducer(params string[] hosts) : base(hosts)
        {

        }
        public RabbitMQProducer(params (string, int)[] hostAndPorts) : base(hostAndPorts)
        {

        }

        #region 普通模式、Work模式
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        /// <param name="options"></param>
        public void Publish(string queue, string message, QueueOptions options = null)
        {
            options = options ?? new QueueOptions();
            var channel = GetChannel();
            channel.QueueDeclare(queue, options.Durable, false, options.AutoDelete, options.Arguments ?? new Dictionary<string, object>());
            var buffer = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", queue, null, buffer);
            channel.Close();
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        /// <param name="configure"></param>
        public void Publish(string queue, string message, Action<QueueOptions> configure)
        {
            QueueOptions options = new QueueOptions();
            configure?.Invoke(options);
            Publish(queue, message, options);
        }
        #endregion

        #region 订阅模式、路由模式、Topic模式
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        /// <param name="options"></param>
        public void Publish(string exchange, string routingKey, string message, ExchangeQueueOptions options = null)
        {
            options = options ?? new ExchangeQueueOptions();
            var channel = GetChannel();
            channel.ExchangeDeclare(exchange, string.IsNullOrEmpty(options.Type) ? RabbitMQExchangeType.Fanout : options.Type, options.Durable, options.AutoDelete, options.Arguments ?? new Dictionary<string, object>());
            if (options.QueueAndRoutingKey != null)
            {
                foreach (var t in options.QueueAndRoutingKey)
                {
                    if (!string.IsNullOrEmpty(t.Item1))
                    {
                        channel.QueueBind(t.Item1, exchange, t.Item2 ?? "", options.BindArguments ?? new Dictionary<string, object>());
                    }
                }
            }
            var buffer = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange, routingKey, null, buffer);
            channel.Close();
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        /// <param name="configure"></param>
        public void Publish(string exchange, string routingKey, string message, Action<ExchangeQueueOptions> configure)
        {
            ExchangeQueueOptions options = new ExchangeQueueOptions();
            configure?.Invoke(options);
            Publish(exchange, routingKey, message, options);
        }
        #endregion
    }
}
