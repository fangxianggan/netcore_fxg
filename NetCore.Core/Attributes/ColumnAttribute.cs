using System;

namespace NetCore.Core.Attributes
{
    /// <summary>
    /// 列字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : BaseAttribute
    {
        /// <summary>
        /// 自增长
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// guid
        /// </summary>
        public bool AutoGuid { set; get; }
        public ColumnAttribute()
        {
            AutoIncrement = false;
            AutoGuid = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoIncrement">是否是自增长</param>
        /// <param name="autoGuid">是否生成guid</param>
        public ColumnAttribute(bool autoIncrement, bool autoGuid)
        {
            AutoIncrement = autoIncrement;
            AutoGuid = autoGuid;
        }

    }

}
