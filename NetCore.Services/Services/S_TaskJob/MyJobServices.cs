using NetCore.Core.Util;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{

    /// <summary>
    /// 
    /// </summary>
    //前一次任务未执行完成时不触发下次
    [DisallowConcurrentExecution]
    public class MyJobServices : IJob, IMyJobServices
    {


        private static ITaskJobServices _taskJobServices
        {
            get
            {
                return (ITaskJobServices)AppConfigUtil._serviceProvider.GetService(typeof(ITaskJobServices));
            }
        }



        public async Task Execute(IJobExecutionContext context)
        {
            //业务
            try
            {
               
                var gId = new Guid(context.JobDetail.Key.Name.Split('|')[0]);
               // LogUtil.Debug("陈大猪。。。" + gId);
                //LogUtil.Debug("ManagerJob Executing ...");
                await _taskJobServices.ExcuteTaskJob(gId);
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                e2.RefireImmediately = true;
            }
            finally
            {
                //LogUtil.Debug("ManagerJob Execute end ");
            }

        }




    }
}
