using NetCore.Core.Enum;

namespace NetCore.EntityModel.QueryModels
{

    #region 公用查询Model
    /// <summary>
    /// 用于存储查询条件的单元
    /// </summary>
    public class ConditionItem
    {
        public ConditionItem() { }

        public ConditionItem(string field, QueryMethod method, object val)
        {
            Field = field;
            Method = method;
            Value = val;
        }

        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 查询方式，用于标记查询方式HtmlName中使用[]进行标识
        /// </summary>
        public QueryMethod Method { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 前缀，用于标记作用域，HTMLName中使用()进行标识
        /// </summary>
        public string Prefix { get; set; }

       

        /// <summary>
        /// 操作符
        /// </summary>
        public string Operator { get; set; }
    }



    #endregion

}
