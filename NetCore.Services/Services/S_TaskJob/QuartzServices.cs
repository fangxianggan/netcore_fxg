using NetCore.Core.Util;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using Quartz.Impl.Matchers;
using System;
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
        }
        public async Task<bool> Add(Type type, JobKey jobKey, ITrigger trigger)
        {
            _scheduler = await _schedulerFactory.GetScheduler();
            var flag = await _scheduler.CheckExists(jobKey);
            if (!flag)
            {
                await _scheduler.Start();
                var job = JobBuilder.Create(type)
                       .WithIdentity(jobKey)
                       .Build();
                await _scheduler.ScheduleJob(job, trigger);
                LogUtil.Debug($"开始任务{jobKey.Group},{jobKey.Name}");

                ////添加监听 监听任务 记录日志
                _scheduler.ListenerManager.AddJobListener(_jobListenerServices, GroupMatcher<JobKey>.GroupEquals(jobKey.Group));
                _scheduler.ListenerManager.AddTriggerListener(_jobListenerServices, GroupMatcher<TriggerKey>.GroupEquals(jobKey.Group));
                //  _scheduler.ListenerManager.AddJobListener(_jobListenerServices, GroupMatcher<JobKey>.AnyGroup());
                ////添加监听 监听触发器 记录日志
                //  _scheduler.ListenerManager.AddTriggerListener(_jobListenerServices, GroupMatcher<TriggerKey>.AnyGroup());

            }
            return !flag;
        }

        public async Task<bool> Delete(JobKey jobKey)
        {
            _scheduler = await _schedulerFactory.GetScheduler();
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
            _scheduler = await _schedulerFactory.GetScheduler();
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
            _scheduler = await _schedulerFactory.GetScheduler();
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
