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
    public class JobListenerServices : IJobListenerServices
    {
        public string Name { get { return "SchedulerJobListener"; } }

      //  public string Name { get; set; }
        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            LogUtil.Debug(string.Format("[{0}]任务监听，name:{1}|任务执行失败重新执行。" , DateTime.Now.ToLongTimeString(), Name));
           // await Console.Error.WriteLineAsync(string.Format("[{0}]任务监听，name:{1}|任务执行失败重新执行。"  , DateTime.Now.ToLongTimeString(), Name));
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            LogUtil.Debug(string.Format("[{0}]任务监听，name:{1}|准备执行任务。", DateTime.Now.ToLongTimeString(), Name));
           // await Console.Error.WriteLineAsync(string.Format("[{0}]任务监听，name:{1}|准备执行任务。", DateTime.Now.ToLongTimeString(), Name));
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            LogUtil.Debug(string.Format("[{0}]任务监听，name:{1}|任务执行完成。", DateTime.Now.ToLongTimeString(), Name));
            //await Console.Error.WriteLineAsync(string.Format("[{0}]任务监听，name:{1}|任务执行完成。"
            //          , DateTime.Now.ToLongTimeString(), Name));
        }

        public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|触发器触发成功。"
                     , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
            //await Console.Error.WriteLineAsync(string.Format("[{0}]触发器监听，name:{1}|触发器触发成功。"
            //         , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
        }

        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|触发器开始触发。"
                    , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
            //await Console.Error.WriteLineAsync(string.Format("[{0}]触发器监听，name:{1}|触发器开始触发。"
            //        , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
        }

        public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|触发器触发失败。"
                   , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
            //await Console.Error.WriteLineAsync(string.Format("[{0}]触发器监听，name:{1}|触发器触发失败。"
            //       , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
        }

        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|可以阻止该任务执行，这里不设阻拦。"
                   , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
            //await Console.Error.WriteLineAsync(string.Format("[{0}]触发器监听，name:{1}|可以阻止该任务执行，这里不设阻拦。"
            //       , DateTime.Now.ToLongTimeString(), trigger.Key.Name));
            // False 时，不阻止该任务。True 阻止执行
            return false;
        }
    }
}
