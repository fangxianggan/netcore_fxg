using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Core.RabbitMQ
{
    public class RabbitHostedService : IHostedService
    {
        RabbitMQConsumerOptions consumerOptions;
        RabbitMQConsumer consumer;

        public RabbitHostedService(IOptions<RabbitMQConsumerOptions> options)
        {
            consumerOptions = options.Value;

            consumer = new RabbitMQConsumer(consumerOptions.Hosts);
            consumer.Password = consumerOptions.Password;
            consumer.UserName = consumerOptions.UserName;
            consumer.VirtualHost = consumerOptions.VirtualHost;
            consumer.Port = consumerOptions.Port;
        }

        /// <summary>
        /// 服务启动
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                consumer.Received += new Action<RecieveResult>(result =>
                {
                    //文件路径
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //文件
                    string fileName = Path.Combine(path, $"{DateTime.Now.ToString("yyyyMMdd")}.log");
                    string message = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}接收到消息：{result.Body}{Environment.NewLine}";
                    File.AppendAllText(fileName, message);

                    //提交
                    result.Commit();
                });

                consumer.Listen(consumerOptions.Queue, options =>
                {
                    options.AutoAck = consumerOptions.AutoAck;
                    options.Arguments = consumerOptions.Arguments;
                    options.AutoDelete = consumerOptions.AutoDelete;
                    options.Durable = consumerOptions.Durable;
                });
            });
        }
        /// <summary>
        /// 服务停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var task = Task.Run(() =>
            {
                consumer.Close();
            });

            await Task.WhenAny(task, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
