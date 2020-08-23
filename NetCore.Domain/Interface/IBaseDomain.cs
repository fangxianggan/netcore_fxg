using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
