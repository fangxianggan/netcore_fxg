using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetCore.Core.RabbitMQ;
using NetCore.Services.IServices.I_RabbitMq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_RabbitMq
{
    public class ProducerMqServices : IProducerMqServices
    {
        private IOptions<RabbitMQLoggerOptions> _options;
        private RabbitMQProducer _producer;
        public ProducerMqServices(IOptions<RabbitMQLoggerOptions> options)
        {
            _options = options;
            var v = _options.Value;
            _producer = new RabbitMQProducer(v.Port, v.Hosts);
            _producer.Password = v.Password;
            _producer.UserName = v.UserName;
            _producer.Port = v.Port;
            _producer.VirtualHost = v.VirtualHost;

        }

        public bool ProducerMesTest()
        {
            var message = "123333";

            var v = _options.Value;

            //发送消息
            _producer.Publish(v.Queue, message, options =>
            {
               // options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };
                options.AutoDelete = v.AutoDelete;
              //  options.Durable = v.Durable;
            });

            return true;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                ProducerMesTest();
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var task = Task.Run(() =>
            {
                _producer.Close();
            });

            await Task.WhenAny(task, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
