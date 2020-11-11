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
        Dictionary<ulong, string> unConfirmedMessageTags = new Dictionary<ulong, string>();
        public ProducerMqServices(IRedisServices redisServices, IOptions<RabbitMQLoggerOptions> options)
        {
            _options = options;
            var v = _options.Value;
            _producer = new RabbitMQProducer(v.Hosts);
            _producer.Password = v.Password;
            _producer.UserName = v.UserName;
            _producer.Port = v.Port;
            _producer.VirtualHost = v.VirtualHost;
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
            _producer._Channel = _producer.GetModelChannel();

      
            Task.Run(() =>
            {
                var v = _options.Value;
                var _Channel = _producer._Channel;

                _Channel.BasicAcks += (sender, ea) => OnBasicAcks(ea.Multiple, ea.DeliveryTag);
                _Channel.BasicNacks += (sender, ea) => OnBasicNacks(ea.Multiple, ea.DeliveryTag);

                ////开启确认模式
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
                for (int i = 0; i < 500000; i++)
                {
                   // Thread.Sleep(100);
                   //Console.WriteLine(i.ToString());
                    // LogUtil.Warn(i.ToString());
                    unConfirmedMessageTags.TryAdd(_Channel.NextPublishSeqNo, i.ToString());
                    _producer.Publish(exchangeName, routeKey, i.ToString(), options =>
                    {
                        options.Type = type;
                        options.QueueAndRoutingKey = new Dictionary<string, string>() { { v.Queue, routeKey } };
                        options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

                    });
                }


                //  _Channel.WaitForConfirmsOrDie();

                // _Channel.TxCommit();

                //  LogUtil.Error("send message failed" + i);

                // _Channel.Close();

            });
            return true;
        }
        private void OnBasicNacks(bool multiple, ulong deliveryTag)
        {
            if (multiple)
            {
                LogUtil.Error("LESS THAN {0} ", deliveryTag);
            }
            else
            {
                LogUtil.Error("{0} ", deliveryTag);
            }
        }

        private void OnBasicAcks(bool multiple, ulong deliveryTag)
        {
            if (multiple)
            {
                var confirmed = unConfirmedMessageTags.Where(k => k.Key <= deliveryTag);
                foreach (var entry in confirmed)
                {
                    unConfirmedMessageTags.Remove(entry.Key);
                  //  LogUtil.Info("with delivery tag {0} ", entry.Key);
                }
            }
            else
            {
                unConfirmedMessageTags.Remove(deliveryTag);
              //  LogUtil.Info("delivery tag {0} ", deliveryTag);
            }
        }
        public bool ProducerMesTest2()
        {
            _producer._Channel = _producer.GetModelChannel();

            Task.Run(() =>
            {
                var v = _options.Value;
                var _Channel = _producer._Channel;

                //开启确认模式
                _Channel.ConfirmSelect();




                //发布消息2
                string exchangeName = "TestTopicChange";
                string routeKey = "TestRouteKey.*";
                var type = RabbitMQExchangeType.Common;
                for (int i = 0; i < 50000; i++)
                {
                    _producer.Publish(exchangeName, routeKey, i.ToString(), options =>
                    {
                        options.Type = type;
                        options.QueueAndRoutingKey = new Dictionary<string, string>() { { v.Queue, routeKey } };
                        options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };

                    });
                }



                _Channel.WaitForConfirmsOrDie();

                // _Channel.TxCommit();

                //  LogUtil.Error("send message failed" + i);



                // _Channel.Close();
            });
            return true;
        }
    }
}
