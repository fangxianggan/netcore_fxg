using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.ProductInfo
{
    public  class ProductInfoViewModel
    {
        public Guid ID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int StockNum { get; set; }
        public int LimitedNum { get; set; }
        public decimal Price { get; set; }
        public string Des { get; set; }
        public string Url { get; set; }

        public DateTime OpeningTime { set; get; }
    }
}
