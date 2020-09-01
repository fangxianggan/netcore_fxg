using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace NetCore.Core.Util
{

    /// <summary>
    /// 
    /// </summary>
    public class QuartzUtil
    {

        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="type">类</param>
        /// <param name="jobKey">键</param>
        /// <param name="trigger">触发器</param>
        public static async Task Add(Type type, JobKey jobKey, ITrigger trigger = null)
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"出现错误1：{ex.Message},栈堆信息1：{ex.InnerException}");
            }
            _scheduler = await _schedulerFactory.GetScheduler();

            await _scheduler.Start();

            if (trigger == null)
            {
                trigger = TriggerBuilder.Create()
                    .WithIdentity("april.trigger")
                    .WithDescription("default")
                    .WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow().WithRepeatCount(-1))
                    .Build();
            }
            try
            {
                var job = JobBuilder.Create(type)
                    .WithIdentity(jobKey)
                    .Build();
                //  LogUtil.Debug($"开启任务{jobKey.Group},{jobKey.Name}");
                await _scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                LogUtil.Error($"出现错误：{ex.Message},栈堆信息：{ex.InnerException}");
            }
        }
        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public static async Task Resume(JobKey jobKey)
        {
            Init();
            _scheduler = await _schedulerFactory.GetScheduler();
            LogUtil.Debug($"恢复任务{jobKey.Group},{jobKey.Name}");
            await _scheduler.ResumeJob(jobKey);
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public static async Task Stop(JobKey jobKey)
        {
            Init();
            _scheduler = await _schedulerFactory.GetScheduler();
            LogUtil.Debug($"暂停任务{jobKey.Group},{jobKey.Name}");
            await _scheduler.PauseJob(jobKey);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private static void Init()
        {
            if (_schedulerFactory == null)
            {
                _schedulerFactory = AppConfigUtil._serviceProvider.GetService<ISchedulerFactory>();
            }
        }
    }
}
