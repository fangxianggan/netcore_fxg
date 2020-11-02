using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.RequestViewModel.ProductInfo
{
    /// <summary>
    /// 抢购产品的实体
    /// </summary>
    public class QGProductViewModel
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public Guid ProId { set; get; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Number { set; get; }
    }
}
