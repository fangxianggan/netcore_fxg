
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.EntityModel.QueryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Interface
{
    public interface IBaseServices<T> where T : class
    {
       
        Task<bool> AddListService(List<T> entity);

        Task<HttpReponseViewModel<T>> AddOrEditService(T entity);

        Task<HttpReponseViewModel<List<T>>> GetPageListService(QueryModel queryModel);
    }
}
