using NetCore.EntityModel.QueryModels;
using NetCore.Repository.Dapper.Entity;
using NetCore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace NetCore.Domain.Interface
{
    public  interface IBaseDomain<T> where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        Task<bool> AddDomain(T entity);
        Task<bool> AddListDomain(List<T> entity);
        Task<bool> EditDomain(T entity);
        Task<bool> DeleteDomain(object id);
        Task<PageData<T>> GetPageList(QueryModel queryModel);
        Task<List<T>> GetList(Expression<Func<T, bool>> whereLambda);
        Task<T>  GetEntity(object keyValues);

        Task<T> GetEntity(Expression<Func<T, bool>> whereLambda);
    }
}
