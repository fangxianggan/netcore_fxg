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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{
    public class TaskJobServices : ITaskJobServices
    {
        private readonly IBaseDomain<TaskJob> _baseDomain;
        private readonly IQuartzServices _quartzServices;

        public TaskJobServices(IBaseDomain<TaskJob> baseDomain, IQuartzServices quartzServices)
        {
            _baseDomain = baseDomain;
            _quartzServices = quartzServices;
        }

     

        public Task<bool> AddListService(List<TaskJobViewModel> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpReponseViewModel<TaskJobViewModel>> AddOrEditService(TaskJobViewModel entity)
        {
            HttpReponseViewModel<TaskJobViewModel> res = new HttpReponseViewModel<TaskJobViewModel>();
            var ent = entity.MapTo<TaskJob>();
            if (ent.ID.IsInitGuid())
            {
                ent.ID = Guid.NewGuid();
                res.Flag = await _baseDomain.AddDomain(ent);
            }
            else
            {
                res.Flag = await _baseDomain.EditDomain(ent);
            }
            if (res.Flag)
            {
                res.Data = entity;
                res.Message = "";
                res.ResultSign = ResultSign.Success;
                res.Code = 20000;
            }

            return res;
        }



        public async Task<HttpReponseViewModel<List<TaskJobViewModel>>> GetPageListService(QueryModel queryModel)
        {
            var pageData = await _baseDomain.GetPageList(queryModel);
            HttpReponseViewModel<List<TaskJobViewModel>> httpReponse = new HttpReponseViewModel<List<TaskJobViewModel>>();
            httpReponse.Code = 20000;
            httpReponse.Data = pageData.DataList.MapToList<TaskJobViewModel>();
            httpReponse.Total = pageData.Total;
            httpReponse.EXESql = pageData.EXESql;
            httpReponse.PageIndex = queryModel.PageIndex;
            httpReponse.PageSize = queryModel.PageSize;
            httpReponse.Flag = true;
            httpReponse.RequestParams = queryModel;
            httpReponse.ResultSign = ResultSign.Success;
            httpReponse.Message = "cj";
            return httpReponse;
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
            var flag= CronExpression.IsValidExpression(cronExpression);
            var errorMsg = "cron表达式错误";
            if (flag) {
                JobKey jobKey = new JobKey(ent.ID + "|" + ent.TaskName, ent.ID + "|" + groupName);
                ITrigger trigger = TriggerBuilder.Create().WithIdentity(ent.ID + "|" + ent.TaskName, ent.ID + "|" + groupName).WithCronSchedule(cronExpression).Build();
                flag = await _quartzServices.Add(typeof(MyJobServices), jobKey, trigger);
                if (flag)
                {
                    errorMsg = "";
                }
                else {
                    errorMsg = "任务启动失败！";
                }
            }
            return new HttpReponseViewModel(flag,errorMsg, "", "");
        }

        /// <summary>
        /// 恢复实例
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<HttpReponseViewModel> ResumeJob(Guid gId)
        {
            var ent= await _baseDomain.GetEntity(gId);
            ent.TaskState =  TaskState.Start.ToInt();
            await _baseDomain.EditDomain(ent);
            JobKey jobKey = new JobKey(ent.ID + "|" + ent.TaskName, ent.ID + "|" + ent.TaskGroup);
            var flag=await _quartzServices.Resume(jobKey);
            var errorMsg = "恢复实例失败！";
            if (flag)
            {
                errorMsg = "";
            }
            return new HttpReponseViewModel(flag,errorMsg);
           
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
            var flag= await _quartzServices.Stop(jobKey);
            var errorMsg = "暂停实例失败！";
            if (flag)
            {
                errorMsg = "";
            }
            return new HttpReponseViewModel(flag,errorMsg);
            
        }

        public async Task<HttpReponseViewModel> DeleteService(object id)
        {
            var ent = await _baseDomain.GetEntity(id);
            JobKey jobKey = new JobKey(ent.ID + "|" + ent.TaskName, ent.ID + "|" + ent.TaskGroup);
            var flag = await _quartzServices.Delete(jobKey);
            var errorMsg = "暂停实例失败！";
            if (flag)
            {
                flag = await _baseDomain.DeleteDomain(id);
                if (flag)
                {
                    errorMsg = "";
                }
                else {
                    errorMsg = "删除数据失败！";
                }
            }
            return new HttpReponseViewModel(flag, errorMsg);
        }
    }
}
