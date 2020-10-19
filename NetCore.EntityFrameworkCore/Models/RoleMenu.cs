using NetCore.EntityFrameworkCore.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.EntityFrameworkCore.Models
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    [Table("RoleMenu")]
    public class RoleMenu : BaseEntity<Guid>
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { set; get; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string  MenuCode { set; get; }

    }
}
