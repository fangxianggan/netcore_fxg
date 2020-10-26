using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.Enum;
using NetCore.DTO.ReponseViewModel.TaskJob;
using NetCore.EntityFrameworkCore.Models;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{
    public class TaskJobServices : BaseServices<TaskJob, TaskJobViewModel>, ITaskJobServices
    {
        private readonly IBaseDomain<TaskJob> _baseDomain;
        private readonly IQuartzServices _quartzServices;

        public TaskJobServices(IBaseDomain<TaskJob> baseDomain, IQuartzServices quartzServices) : base(baseDomain)
        {
            _baseDomain = baseDomain;
            _quartzServices = quartzServices;
        }

        /// <summary>
        ///  启动实例
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        public async Task<HttpReponseViewModel> AddJob(Guid gId)
        {
            var ent = await _baseDomain.GetEntity(gId);
            ent.TaskState = TaskState.Start.ToInt();
            await _baseDomain.EditDomain(ent);
            var groupName = ent.TaskGroup;
            var cronExpression = ent.CronExpression;
            var flag = CronExpression.IsValidExpression(cronExpression);
            var errorMsg = "cron表达式错误";
            if (flag)
            {
                //http请求配置
                var httpDir = new Dictionary<string, string>()
                {
                    { ConstantUtil.REQUESTURL,ent.ApiUrl},
                    { ConstantUtil.REQUESTPARAMETERS,ent.RequestParams},
                    { ConstantUtil.REQUESTTYPE, ent.RequestType.ToString()},
                    { ConstantUtil.HEADERS, ent.RequestHead},
                    { ConstantUtil.MAILMESSAGE, ent.MailMessage.ToString()},
                    { ConstantUtil.UUID, ent.ID.ToString()},
                };

                JobKey jobKey = new JobKey(ent.ID + "|" + ent.TaskName, ent.ID + "|" + groupName);
                DateTimeOffset st1 = ent.StartRunTime.ToDateTimeOffset();
                DateTimeOffset? st2 = null;
                if (!ent.EndRunTime.IsNull())
                {
                    st2 = ent.EndRunTime.Value.ToDateTimeOffset();
                }
                string description = ent.CronExpressionDescription;
                ITrigger trigger = TriggerBuilder.Create().WithIdentity(ent.ID + "|" + ent.TaskName, ent.ID + "|" + groupName).StartAt(st1).EndAt(st2).WithCronSchedule(cronExpression).WithDescription(description).Build();
                flag = await _quartzServices.Add(typeof(MyJobServices), new JobDataMap(httpDir),jobKey, trigger);
                if (flag)
                {
                    errorMsg = "";
                }
                else
                {
                    errorMsg = "任务启动失败！";
                }
            }
            return new HttpReponseViewModel(flag, "");
        }

        /// <summary>
        /// 恢复实例
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<HttpReponseViewModel> ResumeJob(Guid gId)
        {
            var ent = await _baseDomain.GetEntity(gId);
            ent.TaskState = TaskState.Start.ToInt();
            await _baseDomain.EditDomain(ent);
            JobKey jobKey = new JobKey(ent.ID + "|" + ent.TaskName, ent.ID + "|" + ent.TaskGroup);
            var flag = await _quartzServices.Resume(jobKey);
            var errorMsg = "恢复实例失败！";
            if (flag)
            {
                errorMsg = "";
            }
            return new HttpReponseViewModel(flag, errorMsg);

        }


        /// <summary>
        /// 暂停实例
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<HttpReponseViewModel> StopJob(Guid gId)
        {
            var ent = await _baseDomain.GetEntity(gId);
            ent.TaskState = TaskState.Stop.ToInt();
            await _baseDomain.EditDomain(ent);
            JobKey jobKey = new JobKey(ent.ID + "|" + ent.TaskName, ent.ID + "|" + ent.TaskGroup);
            var flag = await _quartzServices.Stop(jobKey);
            var errorMsg = "暂停实例失败！";
            if (flag)
            {
                errorMsg = "";
            }
            return new HttpReponseViewModel(flag, errorMsg);

        }

        /// <summary>
        /// 删除实例 job
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HttpReponseViewModel> DeleteJob(Guid id)
        {
            var ent = await _baseDomain.GetEntity(id);
            JobKey jobKey = new JobKey(ent.ID + "|" + ent.TaskName, ent.ID + "|" + ent.TaskGroup);
            var flag = await _quartzServices.Delete(jobKey);
            var errorMsg = "实例未存在,暂停失败！";
            if (flag)
            {
                flag = await _baseDomain.DeleteDomain(id);
                if (flag)
                {
                    errorMsg = "";
                }
                else
                {
                    errorMsg = "删除数据失败！";
                }
            }
            return new HttpReponseViewModel(flag, errorMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        public async Task<bool> ExcuteTaskJob(Guid gId)
        {
            LogUtil.Debug("ggg11:" + gId);
            var ent = await _baseDomain.GetEntity(gId);
            ent.RunCount = ent.RunCount + 1;
            //var timesOfLoop = 10;   //休眠毫秒
            //Thread.Sleep(timesOfLoop);
            // LogUtil.Debug("ggg:"+JsonUtil.JsonSerialize(ent));
            await _baseDomain.EditDomain(ent);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task JobSchedulerSetUp()
        {
            var list = await _baseDomain.GetList(p => p.TaskState != TaskState.Init.ToInt()&&p.EndRunTime>=DateTime.Now);
            var qdList = list.Where(p => p.TaskState == TaskState.Start.ToInt());
            var ztList = list.Where(p => p.TaskState == TaskState.Stop.ToInt());
           await qdList.ToAsyncEnumerable().ForEachAsync(async item =>
           {

               Thread.Sleep(100);
               await AddJob(item.ID);
           });
            await ztList.ToAsyncEnumerable().ForEachAsync(async item =>
            {
                Thread.Sleep(100);
                await AddJob(item.ID);
                await StopJob(item.ID);
            });

        }

      
    }
}
