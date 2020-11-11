using NetCore.Core.Util;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public abstract class RabbitBase : IDisposable
    {
        List<AmqpTcpEndpoint> amqpList;
        IConnection connection;

        protected RabbitBase(params string[] hosts)
        {
            if (hosts == null || hosts.Length == 0)
            {
                throw new ArgumentException("invalid hosts！", nameof(hosts));
            }
           
            this.amqpList = new List<AmqpTcpEndpoint>();
            this.amqpList.AddRange(hosts.Select(host => new AmqpTcpEndpoint(host, Port)));
        }
        protected RabbitBase( params (string, int)[] hostAndPorts)
        {
            if (hostAndPorts == null || hostAndPorts.Length == 0)
            {
                throw new ArgumentException("invalid hosts！", nameof(hostAndPorts));
            }

            this.amqpList = new List<AmqpTcpEndpoint>();
            this.amqpList.AddRange(hostAndPorts.Select(tuple => new AmqpTcpEndpoint(tuple.Item1, tuple.Item2)));
        }


        public string HostName { set; get; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }= 5672;
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; } = ConnectionFactory.DefaultUser;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = ConnectionFactory.DefaultPass;
        /// <summary>
        /// 虚拟机
        /// </summary>
        public string VirtualHost { get; set; } = ConnectionFactory.DefaultVHost;

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            //connection?.Close();
            //connection?.Dispose();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            connection?.Close();
            connection?.Dispose();
        }

        #region Private
        /// <summary>
        /// 获取rabbitmq的连接
        /// </summary>
        /// <returns></returns>
        protected IModel GetChannel()
        {
            if (connection == null)
            {
                lock (this)
                {
                    if (connection == null)
                    {
                        try
                        {
                            var factory = new ConnectionFactory();
                            factory.Port = Port;
                            factory.UserName = UserName;
                            factory.VirtualHost = VirtualHost;
                            factory.Password = Password;
                            
                         

                            connection = factory.CreateConnection(this.amqpList);
                        } catch (Exception e) {

                            LogUtil.Error(e.Message);
                        }
                    }
                }
            }
            return connection.CreateModel();
        }

        #endregion
    }
}
