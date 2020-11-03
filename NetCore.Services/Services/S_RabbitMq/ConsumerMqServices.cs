﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetCore.Core.RabbitMQ;
using NetCore.Core.Util;
using NetCore.Services.IServices.I_RabbitMq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_RabbitMq
{
    public class ConsumerMqServices : IConsumerMqServices, IHostedService
    {
        private IOptions<RabbitMQConsumerOptions> _options;
        private RabbitMQConsumer _consumer;
        public ConsumerMqServices(IOptions<RabbitMQConsumerOptions> options)
        {
            _options = options;
            var v = _options.Value;
            _consumer = new RabbitMQConsumer(v.Port,v.Hosts);
            _consumer.Password = v.Password;
            _consumer.UserName = v.UserName;
            _consumer.VirtualHost = v.VirtualHost;
            _consumer.Port = v.Port;
        }
        public bool ConsumerMesTest()
        {
            throw new NotImplementedException();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Received += new Action<RecieveResult>(result =>
                {
                    string message = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}接收到消息：{result.Body}{Environment.NewLine}";
                    LogUtil.Info(message);
                    //提交
                    result.Commit();
                });

                var v = _options.Value;
                _consumer.Listen(v.Queue, options =>
                {
                    options.AutoAck = v.AutoAck;
                  //  options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };
                    options.AutoDelete = v.AutoDelete;
                  //  options.Durable = v.Durable;
                });
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var task = Task.Run(() =>
            {
                _consumer.Close();
            });

            await Task.WhenAny(task, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}