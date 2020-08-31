using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.TaskJob;
using NetCore.EntityFrameworkCore.Models;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_TaskJob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{
    public class TaskJobServices : ITaskJobServices
    {
        private readonly IBaseDomain<TaskJob> _baseDomain;

        public TaskJobServices(IBaseDomain<TaskJob> baseDomain)
        {
            _baseDomain = baseDomain;
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
                res.ResultSign = ResultSign.Successful;
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
            httpReponse.ResultSign = ResultSign.Successful;
            httpReponse.Message = "cj";
            return httpReponse;
        }
    }
}
