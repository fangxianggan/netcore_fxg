using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.Enum
{
    public enum EmDataKey
    {
        /// <summary>
        /// 商品列表
        /// </summary>
        ProductInfoHash,
        /// <summary>
        /// 所有订单列表
        /// </summary>
        AllOrderList,
        /// <summary>
        /// 抢购订单队列
        /// </summary>
        QiangOrderEqueue
    }

    /// <summary>
    /// 请求端来源
    /// </summary>
    public enum EmRqSource
    {
        Web = 1,
        Android = 2,
        IOS = 3,
        WebApp = 4
    }

    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum EmOrderStatus
    {
        排队抢购中 = 1000,
        抢购成功 = 2000,
        抢购失败 = 2001
    }
}
