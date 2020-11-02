using NetCore.EntityFrameworkCore.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.EntityFrameworkCore.Models
{
    /// <summary>
    /// 产品信息表
    /// </summary>
    [Table("ProductInfo")]
    public class ProductInfo:BaseEntity<Guid>
    {

        /// <summary>
        /// 产品编码
        /// </summary>
        /// 
        [DisplayName("产品编码"), MaxLength(64)]
        public string ProductCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("产品名称"), MaxLength(64)]
        public string ProductName { get; set; }

        /// <summary>
        /// 库存量
        /// </summary>
        /// 
        [DisplayName("StockNum")]
        public int StockNum { get; set; }
        /// <summary>
        /// 限购数量
        /// </summary>
        /// 
        [DisplayName("限购数量")]
        public int LimitedNum { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary> 
        [DisplayName("产品价格")]
        public decimal Price { get; set; }

        /// <summary>
        /// 产品颜色
        /// </summary>
        /// 
        [DisplayName("产品颜色"), MaxLength(32)]
        public string Color { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        /// 
        [DisplayName("品牌"), MaxLength(32)]
        public string Brand { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        /// 
        [DisplayName("产品描述"), MaxLength(300)]
        public string Des { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        /// 
        [DisplayName("产品图片"), MaxLength(300)]
        public string Image { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        /// 
        [DisplayName("图片详情"), MaxLength(300)]
        public string ImageDetail { get; set; }
        /// <summary>
        /// 开团时间
        /// </summary>
        [DisplayName("开团时间")]
        public DateTime OpeningTime { set; get; }

    }
}
