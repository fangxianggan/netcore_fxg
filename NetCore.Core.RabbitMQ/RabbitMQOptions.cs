using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public abstract class RabbitMQOptions
    {
        /// <summary>
        /// 服务节点
        /// </summary>
        public string[] Hosts { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 虚拟机
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool Durable { get; set; } = true;
        /// <summary>
        /// 是否自动删除
        /// </summary>
        public bool AutoDelete { get; set; } = false;
        /// <summary>
        /// 队列
        /// </summary>
        public string Queue { get; set; }
        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// 交换机类型，放空则为普通模式
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
    public class RabbitMQLoggerOptions : RabbitMQOptions
    {
        /// <summary>
        /// 最低日志记录
        /// </summary>
        public LogLevel MinLevel { get; set; } = LogLevel.Information;
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; } = "Rabbit";
    }
    public class RabbitMQConsumerOptions : RabbitMQOptions
    {
        /// <summary>
        /// 是否自动提交
        /// </summary>
        public bool AutoAck { get; set; } = false;
        /// <summary>
        /// 每次发送消息条数
        /// </summary>
        public ushort? FetchCount { get; set; }
    }
}
