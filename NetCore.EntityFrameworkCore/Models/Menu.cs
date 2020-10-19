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
    /// 菜单表
    /// </summary>
    [Table("Menu")]
    public  class Menu : BaseEntity<Guid>
    {
        /// <summary>
        /// 菜单编码
        /// </summary>
        [DisplayName("菜单编码"), MaxLength(32)]
        public string MenuCode { set; get; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [DisplayName("菜单名称"), MaxLength(32)]
        public string MenuName { set; get; }

        /// <summary>
        /// 路径
        /// </summary>
        [DisplayName("路径"), MaxLength(32)]
        public string Path { set; get; }

        /// <summary>
        /// icon
        /// </summary>
        [DisplayName("icon"), MaxLength(32)]
        public string Icon { set; get; }

        /// <summary>
        /// 父节点
        /// </summary>
        [DisplayName("父节点")]
        public int ParentId { set; get; }

        /// <summary>
        /// 是否掩藏
        /// </summary>
        [DisplayName("是否掩藏")]
        public bool Hidden { set; get; }


        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int Sort { set; get; }


        /// <summary>
        /// 无子节点
        /// </summary>
        [DisplayName("无子节点")]
        public bool NoChildren { set; get; }
    }

}
