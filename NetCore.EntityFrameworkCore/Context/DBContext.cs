using Microsoft.EntityFrameworkCore;
using NetCore.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

        public DbSet<TaskJobLog> TaskJobLogs { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<StoreFiles> StoreFiles { get; set; }
    }
}
