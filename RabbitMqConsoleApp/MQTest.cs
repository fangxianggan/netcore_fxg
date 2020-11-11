using NetCore.Core.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RabbitMqConsoleApp
{
    public class MQTest
    {

        /// <summary>
        /// 普通模式
        /// </summary>
        public void PTMQ()
        {
            //  string[] hosts = new string[] { "192.168.209.133", "192.168.209.134", "192.168.209.135" };
            string[] hosts = new string[] { "127.0.0.1" };
            int port = 5672;
            string userName = "admin";
            string password = "admin";
            string virtualHost = "/";
            string queue = "queue1";
            var arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

            //消费者
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                    });
                }
            }).Start();

            //消息生产
            using (RabbitMQProducer producer = new RabbitMQProducer(hosts))
            {
                producer.UserName = userName;
                producer.Password = password;
                producer.Port = port;
                producer.VirtualHost = virtualHost;

                string message = "";
                do
                {
                    message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                    {
                        break;
                    }
                    producer.Publish(queue, message, options => { options.Arguments = arguments; });

                } while (true);
            }
        }

        /// <summary>
        /// 工作模式
        /// </summary>
        public void WorkMQ()
        {

            //  string[] hosts = new string[] { "192.168.209.133", "192.168.209.134", "192.168.209.135" };
            string[] hosts = new string[] { "127.0.0.1" };
            int port = 5672;
            string userName = "admin";
            string password = "admin";
            string virtualHost = "/";
            string queue = "queue1";
            var arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

            //消费者1
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者1接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                        options.FetchCount = 1;
                    });
                }
            }).Start();

            //消费者2
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者2接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                        options.FetchCount = 2;
                    });
                }
            }).Start();

            //消息生产
            using (RabbitMQProducer producer = new RabbitMQProducer(hosts))
            {
                producer.UserName = userName;
                producer.Password = password;
                producer.Port = port;
                producer.VirtualHost = virtualHost;



                string message = "";
                do
                {
                    message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                    {
                        break;
                    }
                    producer.Publish(queue, message, options => { options.Arguments = arguments; });

                } while (true);
            }
        }

        /// <summary>
        /// 发布订阅模式
        /// </summary>
        public void PDMQ()
        {
            // string[] hosts = new string[] { "192.168.209.133", "192.168.209.134", "192.168.209.135" };
            string[] hosts = new string[] { "127.0.0.1" };
            int port = 5672;
            string userName = "admin";
            string password = "admin";
            string virtualHost = "/";
            string queue1 = "queue1";
            string queue2 = "queue2";
            string exchange = "demo.fanout";
            string exchangeType = RabbitMQExchangeType.Fanout;
            var arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

            //消费者1
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者1接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue1, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                    });
                }
            }).Start();

            //消费者2
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者2接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue2, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                    });
                }
            }).Start();

            //消息生产
            using (RabbitMQProducer producer = new RabbitMQProducer(hosts))
            {
                producer.UserName = userName;
                producer.Password = password;
                producer.Port = port;
                producer.VirtualHost = virtualHost;

                string message = "";
                do
                {
                    message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                    {
                        break;
                    }
                    producer.Publish(exchange, "", message, options => { options.Type = exchangeType; });

                } while (true);
            }
        }

        /// <summary>
        /// 路由模式
        /// </summary>
        public void LYMQ()
        {
            // string[] hosts = new string[] { "192.168.209.133", "192.168.209.134", "192.168.209.135" };
            string[] hosts = new string[] { "127.0.0.1" };
            int port = 5672;
            string userName = "admin";
            string password = "admin";
            string virtualHost = "/";
            string queue1 = "queue1";
            string queue2 = "queue2";
            string exchange = "demo.direct";
            string exchangeType = RabbitMQExchangeType.Direct;
            var arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

            //消费者1
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者1接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue1, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                    });
                }
            }).Start();

            //消费者2
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者2接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue2, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                    });
                }
            }).Start();

            //消息生产
            using (RabbitMQProducer producer = new RabbitMQProducer(hosts))
            {
                producer.UserName = userName;
                producer.Password = password;
                producer.Port = port;
                producer.VirtualHost = virtualHost;

                string message = "";
                int index = 1;
                string[] routes = new string[] { "apple", "banana" };
                do
                {
                    message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                    {
                        break;
                    }
                    var route = routes[index++ % 2];
                    producer.Publish(exchange, route, message, options => { options.Type = exchangeType; });

                } while (true);
            }
        }

        /// <summary>
        /// 主题模式
        /// </summary>
        public void ZTMQ()
        {
            //  string[] hosts = new string[] { "192.168.209.133", "192.168.209.134", "192.168.209.135" };
            string[] hosts = new string[] { "127.0.0.1" };
            int port = 5672;
            string userName = "admin";
            string password = "admin";
            string virtualHost = "/";
            string queue1 = "queue1";
            string queue2 = "queue2";
            string exchange = "demo.topic";
            string exchangeType = RabbitMQExchangeType.Topic;
            var arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

            //消费者1
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者1接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue1, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                    });
                }
            }).Start();

            //消费者2
            new Thread(() =>
            {
                using (RabbitMQConsumer consumer = new RabbitMQConsumer(hosts))
                {
                    consumer.UserName = userName;
                    consumer.Password = password;
                    consumer.Port = port;
                    consumer.VirtualHost = virtualHost;

                    consumer.Received += result =>
                    {
                        Console.WriteLine($"消费者2接收到数据：{result.Body}");
                        result.Commit();//提交
                    };
                    consumer.Listen(queue2, options =>
                    {
                        options.AutoAck = false;
                        options.Arguments = arguments;
                    });
                }
            }).Start();

            //消息生产
            using (RabbitMQProducer producer = new RabbitMQProducer(hosts))
            {
                producer.UserName = userName;
                producer.Password = password;
                producer.Port = port;
                producer.VirtualHost = virtualHost;

                string message = "";
                int index = 1;
                string[] routes = new string[] { "apple.", "banana." };
                do
                {
                    message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                    {
                        break;
                    }
                    var route = routes[index % 2] + index++;
                    producer.Publish(exchange, route, message, options => { options.Type = exchangeType; });

                } while (true);
            }
        }

        /// <summary>
        /// 延迟队列  对消息延迟  消费者
        /// </summary>
        public void SXMQ()
        {
            var factory = new ConnectionFactory() {
                HostName = "127.0.0.1",
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "exchange-direct", type: "direct");
                    string name = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: name, exchange: "exchange-direct", routingKey: "routing-delay");

                    //回调，当consumer收到消息后会执行该函数
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(ea.RoutingKey);
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    //Console.WriteLine("name:" + name);
                    //消费队列"hello"中的消息
                    channel.BasicConsume(queue: name,
                               autoAck: true,
                               consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }

            Console.ReadKey();


        }



    }
}
