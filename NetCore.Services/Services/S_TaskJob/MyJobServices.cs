using NetCore.Core.Util;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{

    /// <summary>
    /// 
    /// </summary>
    //前一次任务未执行完成时不触发下次
    [DisallowConcurrentExecution]
    public class MyJobServices : IJob,IMyJobServices
    {
        public Task Execute(IJobExecutionContext context)
        {

            //业务
            return Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    LogUtil.Debug("陈大猪。。。" + i);
                }

              });
            }

        public Task<bool> UpdateRunCount()
        {
            return Task.Run(() =>
            {
                LogUtil.Debug("hghghghg");
                return true;
            });
           
        }
    }
}
