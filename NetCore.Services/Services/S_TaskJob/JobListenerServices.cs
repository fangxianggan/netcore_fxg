using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.Enum;
using NetCore.DTO.ReponseViewModel.TaskJob;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{
    /// <summary>
    /// 监听 任务监听 触发器监听
    /// </summary>
    public class JobListenerServices : IJobListenerServices
    {

        //private static ITaskJobLogServices _taskJobLogServices
        //{
        //    get
        //    {
        //        return (ITaskJobLogServices)AppConfigUtil._serviceProvider.GetService(typeof(ITaskJobLogServices));
        //    }
        //}




        private static IBaseDomain<TaskJobLog> _baseDomainLog { get { return (IBaseDomain<TaskJobLog>)AppConfigUtil._serviceProvider.GetService(typeof(IBaseDomain<TaskJobLog>)); } }
        private static IBaseDomain<TaskJob> _baseDomain { get { return (IBaseDomain<TaskJob>)AppConfigUtil._serviceProvider.GetService(typeof(IBaseDomain<TaskJob>)); } }

        public string Name { get { return "SchedulerJobListener"; } }




        /// <summary>
        ///1 触发器监听，触发器开始触发
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                //  LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|触发器开始触发。", DateTime.Now.ToLongTimeString(), trigger.Key.Name));
            });
        }

        /// <summary>
        /// 2 任务监听，准备执行任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                //   LogUtil.Debug(string.Format("[{0}]任务监听，name:{1}|准备执行任务。", DateTime.Now.ToLongTimeString(), Name));

            });

        }

        /// <summary>
        ///3 任务监听，任务执行完成
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {

            DateTime NextFireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local);
            DateTime FireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.DateTime, TimeZoneInfo.Local);
            double TotalSeconds = context.JobRunTime.TotalSeconds;
            DateTime endTimeUtc = context.Trigger.EndTimeUtc.Value.DateTime;
            string JobNames = context.JobDetail.Key.Name;
            Guid jobId = new Guid(JobNames.Split('|')[0]);
            string JobName = JobNames.Split('|')[1];
            string LogContent = string.Empty;
            if (context.MergedJobDataMap != null)
            {
                StringBuilder log = new StringBuilder();
                int i = 0;
                foreach (var item in context.MergedJobDataMap)
                {
                    string key = item.Key;
                    if (key.StartsWith("extend_"))
                    {
                        if (i > 0)
                        {
                            log.Append(",");
                        }
                        log.AppendFormat("{0}:{1}", item.Key, item.Value);
                        i++;
                    }
                }
                if (i > 0)
                {
                    LogContent = string.Concat("[", log.ToString(), "]");
                }
            }
            if (jobException != null)
            {
                LogContent = LogContent + " EX:" + jobException.ToString();
            }

            LogUtil.Debug(string.Format("[{0}={2}]任务监听，name:{1}|任务执行完成。", NextFireTimeUtc, jobId, endTimeUtc));

            //var timesOfLoop = 10;   //休眠毫秒
            //Thread.Sleep(timesOfLoop);

            ////更新运行次数
            //var ents = await _baseDomain.GetEntity(jobId);
            //ents.RunCount = ents.RunCount + 1;
            //ents.UpdateTime = DateTime.Now;
            //await _baseDomain.EditDomain(ents);
            //////记录日志
            //await _baseDomainLog.AddDomain(new TaskJobLog()
            //{
            //    ExecutionTime = FireTimeUtc,
            //    ExecutionDuration = TotalSeconds,
            //    ID = Guid.NewGuid(),
            //    JobName = JobName,
            //    RunLog = LogContent,
            //    TaskJobId = jobId
            //});
            ////  任务已经结束 更新状态
            //if (endTimeUtc.ToString() == NextFireTimeUtc.ToString())
            //{
            //    var ent = await _baseDomain.GetEntity(jobId);
            //    ent.TaskState = TaskState.Finsh.ToInt();
            //    ent.UpdateTime = DateTime.Now;
            //    await _baseDomain.EditDomain(ent);
            //}

        }

        /// <summary>
        ///4  触发器监听，触发器触发成功。
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="triggerInstructionCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|触发器触发成功。", DateTime.Now.ToLongTimeString(), trigger.Key.Name));
            });
        }


        /// <summary>
        /// 任务监听，任务执行失败重新执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                LogUtil.Debug(string.Format("[{0}]任务监听，name:{1}|任务执行失败重新执行。", DateTime.Now.ToLongTimeString(), Name));
            });
        }


        /// <summary>
        /// 触发器监听，触发器触发失败
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {

                LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|触发器触发失败。", DateTime.Now.ToLongTimeString(), trigger.Key.Name));

                //写邮件通知


            });
        }

        /// <summary>
        /// 触发器监听|可以阻止该任务执行，这里不设阻拦
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                //  LogUtil.Debug(string.Format("[{0}]触发器监听，name:{1}|可以阻止该任务执行，这里不设阻拦。", DateTime.Now.ToLongTimeString(), trigger.Key.Name));
                // False 时，不阻止该任务。True 阻止执行
                return false;
            });
        }
    }
}
