using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoJobsApplicationBuilderExtensions
    {
        private static IServiceProvider _serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static void JobSchedulerSetUp(this IApplicationBuilder applicationBuilder)
        {
            //_serviceProvider = applicationBuilder.ApplicationServices;
            ////启动定时任务
            //var TaskList = (ITaskJobServices)_serviceProvider.GetService(typeof(ITaskJobServices));
            //TaskList.JobSchedulerSetUp();
            ////添加监听
            //TaskList.GetJobListenerList();

        }
    }
}
