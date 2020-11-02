using NetCore.DTO.ReponseViewModel.ProductInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.RequestViewModel.OrderInfo
{
  public  class MoOrderInfoVM
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// 订单状态 1:正在排队抢购 2：抢购成功 3：抢购失败  EnumHelper.EmOrderStatus
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 支付超时时间  默认下单后30分钟
        /// </summary>
        public DateTime PayOutTime { get; set; }
        /// <summary>
        /// 抢购订单时间
        /// </summary>
        public DateTime CreatTime { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public Guid ProId { get; set; }

        /// <summary>
        /// 购买商品数量
        /// </summary>
        public int Num { get; set; }

        public long UserId { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public ProductInfoViewModel ProductInfo  { get; set; }
    }
}
