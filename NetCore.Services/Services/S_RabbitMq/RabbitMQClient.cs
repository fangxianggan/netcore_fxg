//using Microsoft.Extensions.Options;
//using NetCore.Core.RabbitMQ;
//using NetCore.Core.Util;
//using NetCore.Services.IServices.I_RabbitMq;
//using Newtonsoft.Json;
//using RabbitMQ.Client;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace NetCore.Services.Services.S_RabbitMq
//{
//    public class RabbitMQClient:IRabbitMQClientTest
//    {

//        private readonly IModel _channel;

//        public RabbitMQClient(IOptions<RabbitMQLoggerOptions> options)
//        {
//            try
//            {
//                var factory = new ConnectionFactory()
//                {
//                    HostName = options.Value.Hosts[0],
//                    UserName = options.Value.UserName,
//                    Password = options.Value.Password,
//                   // Port = options.Value.Port,
//                    Port = 5672,
//                };
//                var connection = factory.CreateConnection();
//                _channel = connection.CreateModel();
//            }
//            catch (Exception ex)
//            {
//                LogUtil.Error(ex.Message);

//            }

//        }

//        public virtual void PushMessage(string routingKey, string message)
//        {
//            _channel.ExchangeDeclare(exchange: "message", type: "topic");
          
//            _channel.QueueDeclare(queue: "lemonnovelapi.chapter",
//                                        durable: false,
//                                        exclusive: false,
//                                        autoDelete: false,
//                                        arguments: null);

//            //string msgJson = JsonConvert.SerializeObject(message);
//            var body = Encoding.UTF8.GetBytes(message);
//            _channel.BasicPublish(exchange: "message",
//                                    routingKey: routingKey,
//                                    basicProperties: null,
//                                    body: body);
//        }

//        public string test()
//        {
//            Task.Run(()=> {
//                for (int i=1;i<100000;i++)
//                {
//                    PushMessage("done.task", "hjhj_"+i);
//                }
//            });
           
//            return "";
//        }
//    }
//}
