
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.EntityModel.QueryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Interface
{
    public interface IBaseServices<TView> where TView : class
    {
       
        Task<bool> AddListService(List<TView> entity);

        Task<HttpReponseViewModel> AddOrEditService(TView entity);

        Task<HttpReponseViewModel> DeleteService(object id);

        Task<HttpReponseViewModel<List<TView>>> GetPageListService(QueryModel queryModel);

      
    }
}
