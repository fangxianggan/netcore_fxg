
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.EntityModel.QueryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Interface
{
    public interface IBaseServices<TView> where TView : class
    {
       
        Task<bool> AddListService(List<TView> entity);

        Task<HttpReponseObjViewModel<string>> AddOrEditService(TView entity);

        Task<HttpReponseObjViewModel<string>> AddOrEditService(TView entity,bool isRedis,string hashId);

        Task<HttpReponseViewModel> DeleteService(object id);

        Task<HttpReponseViewModel> DeleteService(object id, bool isRedis, string hashId);

        Task<HttpReponsePageViewModel<List<TView>>> GetPageListService(QueryModel queryModel);

      
    }
}
