using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCore.Core.Util;
using NetCore.Services.IServices.I_TaskJob;

namespace NetCoreApp
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {

        //public static void Main(string[] args)
        //{
        //    CreateWebHostBuilder(args).Build().Run();

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            IWebHost webHost = CreateWebHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var _quartzServices = scope.ServiceProvider.GetRequiredService<IQuartzServices>();
                await _quartzServices.StartScheduleAsync();
                 _quartzServices.AddJobListener();

                var _taskJobServices = scope.ServiceProvider.GetRequiredService<ITaskJobServices>();
                await  _taskJobServices.JobSchedulerSetUp();
            }
            await webHost.RunAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
