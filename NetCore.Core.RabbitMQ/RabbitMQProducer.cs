using NetCore.Core.Util;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    /// <summary>
    /// 生产者
    /// </summary>
    public class RabbitMQProducer : RabbitBase
    {

        public RabbitMQProducer(params string[] hosts) : base(hosts)
        {
           
        }
        public RabbitMQProducer(params (string, int)[] hostAndPorts) : base(hostAndPorts)
        {
          
        }


        public IModel _Channel { set; get; }

        public IModel GetModelChannel()
        {
            _Channel = GetChannel();
            return _Channel;
        }



        public void CloseChannel()
        {
            if (!_Channel.IsClosed)
            {
                _Channel.Close();
            }
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
           
            _Channel.QueueDeclare(queue, options.Durable, false, options.AutoDelete, options.Arguments ?? new Dictionary<string, object>());
            var buffer = Encoding.UTF8.GetBytes(message);



            _Channel.BasicPublish("", queue, null, buffer);



            // channel.Close();
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
         
            _Channel.ExchangeDeclare(exchange, string.IsNullOrEmpty(options.Type) ? RabbitMQExchangeType.Fanout : options.Type, options.Durable, options.AutoDelete, options.Arguments ?? new Dictionary<string, object>());
            if (options.QueueAndRoutingKey != null)
            {
                foreach (var t in options.QueueAndRoutingKey)
                {
                    if (!string.IsNullOrEmpty(t.Key))
                    {
                        _Channel.QueueBind(t.Key, exchange, t.Value ?? "", options.BindArguments ?? new Dictionary<string, object>());
                    }
                }
            }
            var buffer = Encoding.UTF8.GetBytes(message);

         

            _Channel.BasicPublish(exchange, routingKey, true, null, buffer);
            // channel.Close();
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
            Publish(exchange, routingKey, message,  options);
        }
        #endregion


       
    }
}
