using System.Collections.Generic;

namespace NetCore.EntityModel.QueryModels
{
    public class QueryModel
    {
        public QueryModel()
        {
            Items = new List<ConditionItem>();
            OrderList = new List<QueryOrder>();
            Total = 0;
           
        }
        public int PageSize { set; get; }
        public int PageIndex { set; get; }

        public int Total { set; get; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public List<ConditionItem> Items { get; set; }

        /// <summary>
        /// 多个排序
        /// </summary>
        public List<QueryOrder> OrderList
        {
            set;
            get;
        }

        /// <summary>
        /// 是否作为导出
        /// </summary>
        public bool IsReport { set; get; }
    }



}
