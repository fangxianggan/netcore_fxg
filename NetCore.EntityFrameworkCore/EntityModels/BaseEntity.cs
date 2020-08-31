using NetCore.Core.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetCore.EntityFrameworkCore.EntityModels
{
    /// <summary>
    /// 基础类
    /// </summary>
    public class BaseEntity<T>
    {

        /// <summary>
        /// 
        /// </summary>
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        [Key, Id,ColumnAttribute, DisplayName("主键ID")]
        public T ID { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        /// 
        [DisplayName("添加时间")]
        public DateTime? CreateTime { set; get; }

        /// <summary>
        /// 添加人
        /// </summary>
        /// 
        [DisplayName("添加人"), MaxLength(32)]
        public string CreateBy { set; get; }


        /// <summary>
        /// 修改时间
        /// </summary>
        /// 
        [DisplayName("修改时间")]
        public DateTime? UpdateTime { set; get; }


        /// <summary>
        /// 修改人
        /// </summary>
        /// 
        [DisplayName("修改人"), MaxLength(32)]
        public string UpdateBy { set; get; }
    }
}
