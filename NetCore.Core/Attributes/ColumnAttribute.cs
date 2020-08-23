using System;

namespace NetCore.Core.Attributes
{
    /// <summary>
    /// 列字段 自动增长
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public partial class AutoIncrementAttribute : BaseAttribute
    {
        /// <summary>
        /// 自增长
        /// </summary>
        public bool AutoIncrement { get; set; }
        public AutoIncrementAttribute()
        {
            AutoIncrement = false;
        }
        /// <summary>
        /// 是否是自增长
        /// </summary>
        /// <param name="autoIncrement"></param>
        public AutoIncrementAttribute(bool autoIncrement)
        {
            AutoIncrement = autoIncrement;
        }
    }
}
