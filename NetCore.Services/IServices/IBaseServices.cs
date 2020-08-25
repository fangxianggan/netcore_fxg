
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.EntityModel.QueryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Interface
{
    public interface IBaseServices<T> where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        Task<bool> AddService(T entity);

        Task<bool> AddListService(List<T> entity);

        Task<HttpReponseViewModel<List<T>>> GetPageListService(QueryModel queryModel);
    }
}
