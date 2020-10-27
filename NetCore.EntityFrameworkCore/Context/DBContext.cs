using Microsoft.EntityFrameworkCore;
using NetCore.EntityFrameworkCore.Models;

namespace NetCore.EntityFrameworkCore.Context
{
    /// <summary>
    /// 
    /// </summary>
    public class DBContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<TaskJob> TaskJobs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<TaskJobLog> TaskJobLogs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Test> Tests { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<StoreFiles> StoreFiles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Menu> Menus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserInfo> UserInfos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<RoleMenu> RoleMenus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<ProductInfo> ProductInfos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<OrderInfo> OrderInfos { get; set; }

    }
}
