using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.RabbitMQ
{
    public static class RabbitMQExchangeType
    {
        /// <summary>
        /// 普通模式
        /// </summary>
        public const string Common = "";
        /// <summary>
        /// 路由模式
        /// </summary>
        public const string Direct = "direct";
        /// <summary>
        /// 发布/订阅模式
        /// </summary>
        public const string Fanout = "fanout";
        /// <summary>
        /// 匹配订阅模式
        /// </summary>
        public const string Topic = "topic";
    }
}
