using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public class ExchangeQueueOptions : QueueOptions
    {
        /// <summary>
        /// 交换机类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 队列及路由值
        /// </summary>
        //public (string, string)[] QueueAndRoutingKey { get; set; }
        // 
        public IDictionary<string, string> QueueAndRoutingKey { get; set; }= new Dictionary<string, string>();
        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> BindArguments { get; set; } = new Dictionary<string, object>();
    }
}
