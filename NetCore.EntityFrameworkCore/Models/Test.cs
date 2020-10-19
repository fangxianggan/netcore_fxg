using NetCore.EntityFrameworkCore.EntityModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.EntityFrameworkCore.Models
{
    /// <summary>
    /// 测试
    /// </summary>
    [Table("Test")]
    public class Test : BaseEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
    }
}
