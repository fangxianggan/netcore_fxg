using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public class ConsumeQueueOptions: QueueOptions
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
