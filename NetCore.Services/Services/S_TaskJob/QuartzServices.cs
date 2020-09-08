using Microsoft.CodeAnalysis.Differencing;
using NetCore.Core.Util;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{
    public class QuartzServices : IQuartzServices
    {
        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;
        private static IJobListenerServices _jobListenerServices;
        public QuartzServices(ISchedulerFactory schedulerFactory, IJobListenerServices jobListenerServices)
        {
            _schedulerFactory = schedulerFactory;
            _jobListenerServices = jobListenerServices;
            _scheduler = _schedulerFactory.GetScheduler().Result;
        }

        
        /// <summary>
        /// 开启调度器
        /// </summary>
        /// <returns></returns>
        public async Task StartScheduleAsync()
        {
             await _scheduler.Start();
             LogUtil.Debug("任务调度启动！");
        }


        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddJobListener()
        {
            _scheduler.ListenerManager.AddJobListener(_jobListenerServices, GroupMatcher<JobKey>.AnyGroup());
            //添加监听 监听触发器 记录日志
            _scheduler.ListenerManager.AddTriggerListener(_jobListenerServices, GroupMatcher<TriggerKey>.AnyGroup());

            ////添加监听
            //_scheduler.ListenerManager.AddJobListener(_jobListenerServices);
            ////添加监听 监听触发器 记录日志
            //_scheduler.ListenerManager.AddTriggerListener(_jobListenerServices);
        }

        public async Task<bool> Add(Type type,JobDataMap keyValues, JobKey jobKey, ITrigger trigger)
        {

            var flag = await _scheduler.CheckExists(jobKey);
            if (!flag)
            {

                  //  await _scheduler.Start();
                var job = JobBuilder.Create(type)
                        .WithIdentity(jobKey).SetJobData(keyValues).Build();
                // 


                await _scheduler.ScheduleJob(job, trigger);
                // LogUtil.Debug($"开始任务{jobKey.Group},{jobKey.Name}");

                //await Task.Run(() =>
                // {
                //  var timesOfLoop = 100;   //休眠毫秒
                //   Thread.Sleep(timesOfLoop);

                //_scheduler.ListenerManager.AddJobListener(_jobListenerServices, GroupMatcher<JobKey>.AnyGroup());
                ////添加监听 监听触发器 记录日志
                //_scheduler.ListenerManager.AddTriggerListener(_jobListenerServices, GroupMatcher<TriggerKey>.AnyGroup());
                //////添加监听 监听任务 记录日志
                //Thread.Sleep(1000);
                //_scheduler.ListenerManager.AddJobListener(_jobListenerServices, KeyMatcher<JobKey>.KeyEquals(new JobKey(jobKey.Group, jobKey.Name)));
                //_scheduler.ListenerManager.AddTriggerListener(_jobListenerServices, KeyMatcher<TriggerKey>.KeyEquals(new TriggerKey(trigger.JobKey.Group, trigger.JobKey.Name)));

                //  });






            }
            return !flag;
        }



        public async Task<bool> Delete(JobKey jobKey)
        {

            var flag = await _scheduler.CheckExists(jobKey);
            if (flag)
            {
                LogUtil.Debug($"删除任务{jobKey.Group},{jobKey.Name}");
                flag = await _scheduler.DeleteJob(jobKey);
            }
            return flag;
        }

        public async Task<bool> Resume(JobKey jobKey)
        {

            var flag = await _scheduler.CheckExists(jobKey);
            if (flag)
            {
                LogUtil.Debug($"恢复任务{jobKey.Group},{jobKey.Name}");
                await _scheduler.ResumeJob(jobKey);
            }
            return flag;
        }

        public async Task<bool> Stop(JobKey jobKey)
        {
            var flag = await _scheduler.CheckExists(jobKey);
            if (flag)
            {
                LogUtil.Debug($"暂停任务{jobKey.Group},{jobKey.Name}");
                await _scheduler.PauseJob(jobKey);
            }
            return flag;
        }


    }
}
