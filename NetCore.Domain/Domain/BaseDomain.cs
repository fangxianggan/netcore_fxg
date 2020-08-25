using NetCore.Domain.Interface;
using NetCore.EntityModel.QueryModels;
using NetCore.Repository.Dapper.Entity;
using NetCore.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Domain.Domain
{
    public class BaseDomain<T> : IBaseDomain<T> where T : class, new()
    {
       
        private readonly IRepository<T> _repository;
        public BaseDomain(IRepository<T> repository)
        {
            _repository = repository;
        }
        public  async Task<bool> AddDomain(T entity)
        {
            var dd= await _repository.Add(entity);
            return dd; 
        }

        public async Task<bool> AddListDomain(List<T> entity)
        {
            var dd = await _repository.AddList(entity)>0?true:false;
            return dd;
        }

        public async Task<PageData<T>> GetPageList(QueryModel queryModel)
        {
            var list = await _repository.GetPageList(queryModel);
            return list;
        }
    }
}
