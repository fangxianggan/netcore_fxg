using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public class QueueOptions
    {
        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool Durable { get; set; } = true;
        /// <summary>
        /// 是否自动删除
        /// </summary>
        public bool AutoDelete { get; set; } = false;
        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}
