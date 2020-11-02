using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetCore.Core.RabbitMQ
{
    public class RabbitMQConsumer : RabbitBase
    {
        public RabbitMQConsumer(params string[] hosts) : base(hosts)
        {

        }
        public RabbitMQConsumer(params (string, int)[] hostAndPorts) : base(hostAndPorts)
        {

        }

        public event Action<RecieveResult> Received;

        /// <summary>
        /// 构造消费者
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private IBasicConsumer ConsumeInternal(IModel channel, ConsumeQueueOptions options)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                try
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    if (!options.AutoAck)
                    {
                        cancellationTokenSource.Token.Register(() =>
                        {
                            channel.BasicAck(e.DeliveryTag, false);
                        });
                    }
                    Received?.Invoke(new RecieveResult(e, cancellationTokenSource));
                }
                catch { }
            };
            if (options.FetchCount != null)
            {
                channel.BasicQos(0, options.FetchCount.Value, false);
            }
            return consumer;
        }

        #region 普通模式、Work模式
        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="options"></param>
        public ListenResult Listen(string queue, ConsumeQueueOptions options = null)
        {
            options = options ?? new ConsumeQueueOptions();
            var channel = GetChannel();
            channel.QueueDeclare(queue, options.Durable, false, options.AutoDelete, options.Arguments ?? new Dictionary<string, object>());
            var consumer = ConsumeInternal(channel, options);
            channel.BasicConsume(queue, options.AutoAck, consumer);
            ListenResult result = new ListenResult();
            result.Token.Register(() =>
            {
                try
                {
                    channel.Close();
                    channel.Dispose();
                }
                catch { }
            });
            return result;
        }
        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="configure"></param>
        public ListenResult Listen(string queue, Action<ConsumeQueueOptions> configure)
        {
            ConsumeQueueOptions options = new ConsumeQueueOptions();
            configure?.Invoke(options);
            return Listen(queue, options);
        }
        #endregion
        #region 订阅模式、路由模式、Topic模式
        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="options"></param>
        public ListenResult Listen(string exchange, string queue, ExchangeConsumeQueueOptions options = null)
        {
            options = options ?? new ExchangeConsumeQueueOptions();
            var channel = GetChannel();
            channel.QueueDeclare(queue, options.Durable, false, options.AutoDelete, options.Arguments ?? new Dictionary<string, object>());
            if (options.RoutingKeys != null && !string.IsNullOrEmpty(exchange))
            {
                foreach (var key in options.RoutingKeys)
                {
                    channel.QueueBind(queue, exchange, key, options.BindArguments);
                }
            }
            var consumer = ConsumeInternal(channel, options);
            channel.BasicConsume(queue, options.AutoAck, consumer);
            ListenResult result = new ListenResult();
            result.Token.Register(() =>
            {
                try
                {
                    channel.Close();
                    channel.Dispose();
                }
                catch { }
            });
            return result;
        }
        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="configure"></param>
        public ListenResult Listen(string exchange, string queue, Action<ExchangeConsumeQueueOptions> configure)
        {
            ExchangeConsumeQueueOptions options = new ExchangeConsumeQueueOptions();
            configure?.Invoke(options);
            return Listen(exchange, queue, options);
        }
        #endregion
    }
    public class RecieveResult
    {
        CancellationTokenSource cancellationTokenSource;
        public RecieveResult(BasicDeliverEventArgs arg, CancellationTokenSource cancellationTokenSource)
        {
           
            this.Body = Encoding.UTF8.GetString(arg.Body.ToArray());
            this.ConsumerTag = arg.ConsumerTag;
            this.DeliveryTag = arg.DeliveryTag;
            this.Exchange = arg.Exchange;
            this.Redelivered = arg.Redelivered;
            this.RoutingKey = arg.RoutingKey;
            this.cancellationTokenSource = cancellationTokenSource;
        }

        /// <summary>
        /// 消息体
        /// </summary>
        public string Body { get; private set; }
        /// <summary>
        /// 消费者标签
        /// </summary>
        public string ConsumerTag { get; private set; }
        /// <summary>
        /// Ack标签
        /// </summary>
        public ulong DeliveryTag { get; private set; }
        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; private set; }
        /// <summary>
        /// 是否Ack
        /// </summary>
        public bool Redelivered { get; private set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string RoutingKey { get; private set; }

        public void Commit()
        {
            if (cancellationTokenSource == null || cancellationTokenSource.IsCancellationRequested) return;

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
    public class ListenResult
    {
        CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// CancellationToken
        /// </summary>
        public CancellationToken Token { get { return cancellationTokenSource.Token; } }
        /// <summary>
        /// 是否已停止
        /// </summary>
        public bool Stoped { get { return cancellationTokenSource.IsCancellationRequested; } }

        public ListenResult()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
