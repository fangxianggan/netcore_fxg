using Microsoft.Extensions.Options;
using NetCore.Core.RabbitMQ;
using NetCore.Core.Util;
using NetCore.Services.IServices.I_RabbitMq;
using NetCore.Services.IServices.I_Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_RabbitMq
{
    public class ProducerMqServices : IProducerMqServices
    {
        private IOptions<RabbitMQLoggerOptions> _options;
        private RabbitMQProducer _producer;
        private IRedisServices _redisServices;
        public ProducerMqServices(IRedisServices redisServices, IOptions<RabbitMQLoggerOptions> options)
        {
            _options = options;
            var v = _options.Value;
            _producer = new RabbitMQProducer(v.Hosts);
            _producer.Password = v.Password;
            _producer.UserName = v.UserName;
            _producer.Port = v.Port;
            _producer.VirtualHost = v.VirtualHost;

            _producer._Channel = _producer.GetModelChannel();
            _redisServices = redisServices;

        }

        public bool ProducerMesTest()
        {
            //_redisServices.hashId = "jayuser";
            //var hashList = _redisServices.GetAllHashById();


            //ConcurrentBag<string> list = new ConcurrentBag<string>();
            //Parallel.ForEach(hashList, item =>
            //{
            //    list.Add(item.Key);
            //});


            //ConcurrentBag<int> list = new ConcurrentBag<int>();
            //Parallel.For(0, 100000, item =>
            //{
            //    list.Add(item);
            //});

            Task.Run(() =>
            {




                var message = "123333";

                var v = _options.Value;


                var _Channel = _producer._Channel;

                //开启确认模式
                _Channel.ConfirmSelect();


                //for (int i = 0; i < 20000; i++)
                //{

                //_redisServices.hashId = "kkk";
                //_redisServices.SetValueHash(i.ToString(),i.ToString());

                ////  发送消息
                //_producer.Publish(v.Queue, i.ToString(), options =>
                // {
                //     options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };
                //     options.AutoDelete = v.AutoDelete;
                //     options.Durable = v.Durable;

                // });

                //发布消息2
                string exchangeName = "TestTopicChange";
                string routeKey = "TestRouteKey.*";
                var type = RabbitMQExchangeType.Direct;
                for (int i = 0; i < 50000; i++)
                {
                    _producer.Publish(exchangeName, routeKey, i.ToString(), options =>
                    {
                        options.Type = type;
                        options.QueueAndRoutingKey = new Dictionary<string, string>() { { v.Queue, routeKey } };
                        options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

                    });
                }

                // Thread.Sleep(10);
                // }
              
                _Channel.WaitForConfirmsOrDie();

                // _Channel.TxCommit();

                //  LogUtil.Error("send message failed" + i);


                // _Channel.Close();
                //if (!_Channel.WaitForConfirms())
                //{

                //}
                // _Channel.Close();

                //发布消息2
                //string exchangeName = "TestTopicChange";
                //string routeKey = "TestRouteKey.*";
                //var type = RabbitMQExchangeType.Topic;
                //for (int i = 0; i < 100000; i++)
                //{
                //    _producer.Publish(exchangeName, type, i.ToString(), options =>
                //    {
                //        options.Type = v.Type;
                //        options.QueueAndRoutingKey = new Dictionary<string, string>() { { v.Queue, routeKey } };
                //        options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

                //    });
                //}




            });




            return true;
        }


    }
}
