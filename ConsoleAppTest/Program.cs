using RabbitMQ.Client;
using ServiceStack.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest
{
    class Program
    {
        /// <summary>
        // 延迟队列  对消息延迟  场景 生成订单 未支付 30分钟之后释放订单
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            ///生产者
            var factory = new ConnectionFactory() { HostName = "127.0.0.1", UserName = "admin", Password = "admin" };
            using (var connection = factory.CreateConnection())
            {
                while (Console.ReadLine() != null)
                {
                    using (var channel = connection.CreateModel())
                    {

                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        //dic.Add("x-expires", 30000);
                        //dic.Add("x-message-ttl", 10000);//队列上消息过期时间，应小于队列过期时间 
                        dic.Add("x-dead-letter-exchange", "exchange-direct");//过期消息转向路由 
                        dic.Add("x-dead-letter-routing-key", "routing-delay");//过期消息转向路由相匹配routingkey 
                                                                              //创建一个名叫"zzhello"的消息队列
                        channel.QueueDeclare(queue: "zzhello",
                          durable: true,
                          exclusive: false,
                          autoDelete: false,
                          arguments: dic);

                        for (int i = 0; i < 40000; i++)
                        {
                            var message = "Hello World!" + i.ToString();
                            var body = Encoding.UTF8.GetBytes(message);


                            var properties = channel.CreateBasicProperties();
                            properties.Expiration = "10000"; //每个消息只有10秒时间
                           

                            //向该消息队列发送消息message
                            channel.BasicPublish(exchange: "",
                              routingKey: "zzhello",
                              basicProperties: properties,
                              body: body);

                            Console.WriteLine(" [x] Sent {0}", message);
                        }
                    }
                }
            }

            Console.ReadKey();

        }
    }
}
