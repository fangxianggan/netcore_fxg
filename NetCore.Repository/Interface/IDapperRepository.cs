using NetCore.EntityModel.QueryModels;
using NetCore.Repository.Dapper.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Repository.Interface
{
    public interface IDapperRepository<T> where T : class
    {
        
        Task<bool> Add(T entity);

        Task<int> AddList(List<T> list);

        Task<bool> Edit(T entity);

        Task<bool> Delete(string ids);

        Task<bool> Delete(object keyValues);

        Task<bool> Delete(Expression<Func<T, bool>> whereLambda);

        Task<bool> InsertUpdateOrDelete(string sql, dynamic parms = null);

        Task<bool> InsertUpdateOrDelete(string sql, dynamic parms = null, CommandType? commandType = null);

        Task<T> GetEntity( object keyValues);

        Task<T> GetEntity(Expression<Func<T, bool>> whereLambda);

        Task<T> GetEntity(string sql, dynamic parms = null);

        Task<T> GetEntity(string procName, dynamic parms = null, CommandType? commandType = null);

        Task<List<T>> GetList(Expression<Func<T, bool>> whereLambda);

        Task<List<T>> GetList(string sql, dynamic parms=null);

        Task<List<T>> GetList(string procName, dynamic parms = null, CommandType? commandType = null);

        Task<DataTable> GetDataTable(Expression<Func<T, bool>> whereLambda);

        Task<DataTable> GetDataTable(string sql, dynamic parms = null);

        Task<PageData<T>> GetPageList(QueryModel queryModel);

        Task<PageData<T>> GetPageList(string sql,string countSql="", dynamic parms=null);

        Task<PageData<T>> GetPageMultList(Type[] types, Func<object[], T> map, string sql, string countSql = "", object param = null, string splitOn = "Id");
      

    }
}
