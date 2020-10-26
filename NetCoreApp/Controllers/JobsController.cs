using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.TaskJob;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_TaskJob;
using NetCoreApp.Filters;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 任务管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskJobServices _taskJobServices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskJobServices"></param>
        public JobsController(ITaskJobServices taskJobServices)
        {
            _taskJobServices = taskJobServices;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [TypeFilter(typeof(CustomerExceptionFilter))]
        [HttpPost, Route("GetPageList")]
        public async Task<HttpReponsePageViewModel<List<TaskJobViewModel>>> GetPageList(QueryModel queryModel)
        {
            return await Task.Run(() =>
            {
                return _taskJobServices.GetPageListService(queryModel);
            });

        }

        /// <summary>
        /// 新增修改任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [TypeFilter(typeof(CustomerExceptionFilter))]
        [HttpPost, Route("AddOrEditTaskJob")]
        public async Task<HttpReponseViewModel> AddOrEditTaskJob(TaskJobViewModel model)
        {
            return await Task.Run(() =>
            {
                return _taskJobServices.AddOrEditService(model);
            });
        }

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        ///
        [TypeFilter(typeof(CustomerExceptionFilter))]
        [HttpPost, Route("DeleteTaskJob")]
        public async Task<HttpReponseViewModel> DeleteTaskJob([FromBody] Guid gId)
        {
            return await _taskJobServices.DeleteService(gId);
        }


        /// <summary>
        /// 启动实例
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        /// 
        [TypeFilter(typeof(CustomerExceptionFilter))]
        [HttpGet, Route("AddJob")]
        public async Task<HttpReponseViewModel> AddJob(Guid gId)
        {
            return await Task.Run(() =>
            {
                return _taskJobServices.AddJob(gId);
            });
        }

        /// <summary>
        /// 恢复实例
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        /// 
        [HttpGet, Route("ResumeJob")]
        public async Task<HttpReponseViewModel> ResumeJob(Guid gId)
        {
            return await Task.Run(() =>
            {
                return _taskJobServices.ResumeJob(gId);
            });
        }

        /// <summary>
        /// 停止实例
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        /// 
        [HttpGet, Route("StopJob")]
        public async Task<HttpReponseViewModel> StopJob(Guid gId)
        {
            return await Task.Run(() =>
            {
                return _taskJobServices.StopJob(gId);
            });
        }



    }
}