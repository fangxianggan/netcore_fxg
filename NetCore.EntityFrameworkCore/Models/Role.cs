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
    /// 角色表
    /// </summary>
    [Table("Role")]
    public class Role:BaseEntity<Guid>
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        [MaxLength(32), Required, DisplayName("角色编码")]
        public string RoleCode { set; get; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [MaxLength(32), Required, DisplayName("角色名称")]
        public string RoleName { set; get; }
    }
}
