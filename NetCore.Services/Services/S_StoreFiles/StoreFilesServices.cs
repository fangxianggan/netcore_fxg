using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.EntityFrameworkCore.Models;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_StoreFiles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_StoreFiles
{
    public class StoreFilesServices : IStoreFilesServices
    {
        private readonly IBaseDomain<StoreFiles> _baseDomain;

        public StoreFilesServices(IBaseDomain<StoreFiles> baseDomain)
        {
            _baseDomain = baseDomain;
        }
        public Task<bool> AddListService(List<StoreFilesViewModel> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpReponseViewModel<List<StoreFilesViewModel>>> GetPageListService(QueryModel queryModel)
        {
            var pageData = await _baseDomain.GetPageList(queryModel);
            HttpReponseViewModel<List<StoreFilesViewModel>> httpReponse = new HttpReponseViewModel<List<StoreFilesViewModel>>();
            httpReponse.Code = 20000;
            httpReponse.Data = pageData.DataList.MapToList<StoreFilesViewModel>();
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

        public Task<HttpReponseViewModel<StoreFilesViewModel>> AddOrEditService(StoreFilesViewModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
