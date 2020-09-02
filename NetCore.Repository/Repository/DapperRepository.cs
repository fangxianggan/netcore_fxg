using NetCore.EntityModel.QueryModels;
using NetCore.Repository.Dapper;
using NetCore.Repository.Dapper.Entity;
using NetCore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCore.Repository.Repository
{
    public class DapperRepository<T> : BaseRepository, IDapperRepository<T>
          where T : class, new()
    {

        private readonly IUnitOfWork _unitOfWork;
        public DapperRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Add(T entity)
        {
            return await DapperExtensions.Insert(_unitOfWork, entity) > 0 ? true : false;
        }

        public async Task<int> AddList(List<T> list)
        {
            return await DapperExtensions.SqlBulkCopy(_unitOfWork, list);
        }
        public async Task<bool> Edit(T entity)
        {
            return await DapperExtensions.Update(_unitOfWork, entity) > 0 ? true : false;
        }

        public async Task<bool> Delete(string ids)
        {
            return await DapperExtensions.DeleteByIds<T>(_unitOfWork, ids) > 0 ? true : false;
        }

        public async Task<bool> Delete(object keyValues)
        {
            return await DapperExtensions.DeleteById<T>(_unitOfWork, keyValues) > 0 ? true : false;
        }

        public async Task<bool> Delete(Expression<Func<T, bool>> whereLambda)
        {
            return await DapperExtensions.Delete<T>(_unitOfWork, whereLambda) > 0 ? true : false;
        }

        public async Task<bool> InsertUpdateOrDelete(string sql, dynamic parms = null)
        {
            return await DapperExtensions.InsertUpdateOrDelete(_unitOfWork,sql,parms)>0?true:false;
        }

        public async Task<bool> InsertUpdateOrDelete(string sql, dynamic parms = null, CommandType? commandType =null)
        {
            return await DapperExtensions.InsertUpdateOrDelete(_unitOfWork, sql, parms, commandType) > 0 ? true : false;
        }

        public async Task<T> GetEntity(object keyValues)
        {
            return await DapperExtensions.SingleOrDefault<T>(_unitOfWork, keyValues);
        }

        public async Task<T> GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            return await DapperExtensions.SingleOrDefault(_unitOfWork, whereLambda);
        }

        public async Task<T> GetEntity(string sql, dynamic parms = null)
        {
            return await DapperExtensions.SingleOrDefault(_unitOfWork, sql, parms);
        }

        public async Task<T> GetEntity(string procName, dynamic parms = null, CommandType? commandType = null)
        {
            return await DapperExtensions.SingleOrDefault(_unitOfWork, procName, parms, commandType);
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return await DapperExtensions.Query(_unitOfWork, whereLambda);
        }
        public async Task<List<T>> GetList(string sql, dynamic parms = null)
        {
            return await DapperExtensions.Query<T>(_unitOfWork, sql, parms);
        }
        public async Task<List<T>> GetList(string procName, dynamic parms = null, CommandType? commandType = null)
        {
            return await DapperExtensions.Query<T>(_unitOfWork, procName, parms, commandType);
        }

        public async Task<DataTable> GetDataTable(Expression<Func<T, bool>> whereLambda)
        {
            return await DapperExtensions.QueryDataTable<T>(_unitOfWork, whereLambda);
        }

        public async Task<DataTable> GetDataTable(string sql, dynamic parms = null)
        {
            return await DapperExtensions.QueryDataTable(_unitOfWork,sql,parms);
        }
        public async Task<PageData<T>> GetPageList(QueryModel queryModel)
        {
            return await DapperExtensions.PageQuery<T>(_unitOfWork, queryModel);
        }

        public async Task<PageData<T>> GetPageList(string sql, string countSql = "", dynamic parms = null)
        {
            return await DapperExtensions.PageQuery<T>(_unitOfWork, sql, countSql, parms);
        }

        public async Task<PageData<T>> GetPageMultList(Type[] types, Func<object[], T> map, string sql, string countSql = "", object param = null, string splitOn = "Id")
        {
            return await DapperExtensions.PageQueryMult<T>(_unitOfWork,types,map,sql,countSql,param,splitOn);
        }

    }
}
