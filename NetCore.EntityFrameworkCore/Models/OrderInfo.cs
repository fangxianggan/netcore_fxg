using NetCore.EntityFrameworkCore.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.EntityFrameworkCore.Models
{
    /// <summary>
    /// 订单信息
    /// </summary>
    [Table("OrderInfo")]
    public class OrderInfo:BaseEntity<Guid>
    {
        /// <summary>
        /// 订单状态 1:正在排队抢购 2：抢购成功 3：抢购失败  EnumHelper.EmOrderStatus
        /// </summary>
        /// 
        [DisplayName("订单状态")]
        public int OrderStatus { get; set; }
        /// <summary>
        /// 支付超时时间  默认下单后30分钟
        /// </summary>
        /// 
        [DisplayName("支付超时时间")]
        public DateTime PayOutTime { get; set; }

        /// <summary>
        /// 产品的id
        /// </summary>
        /// 
        [DisplayName("产品id")]
        public Guid ProductId { set; get; }

        /// <summary>
        /// 购买数量
        /// </summary>
        /// 
        [DisplayName("购买数量")]
        public int PurchaseNum { set; get; }

      

    }
}
