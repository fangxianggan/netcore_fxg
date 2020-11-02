using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public class ExchangeConsumeQueueOptions : ConsumeQueueOptions
    {
        /// <summary>
        /// 路由值
        /// </summary>
        public string[] RoutingKeys { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> BindArguments { get; set; } = new Dictionary<string, object>();
    }
}
