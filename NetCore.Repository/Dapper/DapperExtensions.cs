using Dapper;
using MySql.Data.MySqlClient;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Core.Helper;
using NetCore.Core.Util;
using NetCore.EntityModel.QueryModels;
using NetCore.Repository.Dapper.Entity;
using NetCore.Repository.Interface;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Repository.Dapper
{
    public class DapperExtensions
    {

        #region 实体执行

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static async Task<int> Insert<T>(IUnitOfWork unitOfWork, T t, SqlQuery sql = null)
            where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            int result = await unitOfWork.DbConnection.ExecuteAsync(sql.InsertSql, t, unitOfWork.DbTransaction);
            stopwatch.Stop();
            return result;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="entitys"></param>
        /// <returns></returns>
    
        public static async Task<int> SqlBulkCopy<T>(IUnitOfWork unitOfWork, List<T> entitys) where T : new()
        {
            int result = 0;
            if (unitOfWork.DbType == DBType.SqlServer)
            {
                var destinationConnection = unitOfWork.DbConnection as SqlConnection;
                var transaction = unitOfWork.DbTransaction as SqlTransaction;
                using (var bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.CheckConstraints, transaction))
                {
                    Type type = entitys[0].GetType();
                    object classAttr = type.GetCustomAttributes(false)[0];
                    if (classAttr is TableAttribute)
                    {
                        TableAttribute tableAttr = classAttr as TableAttribute;
                        bulkCopy.DestinationTableName = tableAttr.Name; //要插入的表的表明 
                    }
                    DataTable dt = DTListConvertUtil<T>.FillDataTable(entitys);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        await bulkCopy.WriteToServerAsync(dt);
                    }
                    result = entitys.Count();
                }
            }
            else
            {
                var destinationConnection =(ProfiledDbConnection) unitOfWork.DbConnection;
                DataTable table = DTListConvertUtil<T>.FillDataTable(entitys);
                string tmpPath = Directory.GetCurrentDirectory() + "\\UpTemp";
                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);
                tmpPath = Path.Combine(tmpPath, "Temp.csv");//csv文件临时目录
                string csv = DTListConvertUtil<T>.DataTableToCsv(table);
                //异步文件读取
                var bits = Encoding.UTF8.GetBytes(csv);
                using (var fs = new FileStream(
      path: tmpPath,
      mode: FileMode.Create,
      access: FileAccess.Write,
      share: FileShare.None,
      bufferSize: 4096,
      useAsync: true))
                {
                    await fs.WriteAsync(bits, 0, bits.Length);
                    
                }
                
                MySqlBulkLoader bulk = new MySqlBulkLoader((MySqlConnection)destinationConnection.WrappedConnection)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n",
                    FileName = tmpPath,
                    NumberOfLinesToSkip = 0,
                    TableName = table.TableName,
                };
                result = await bulk.LoadAsync();

                File.Delete(tmpPath);
                
                
            }

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static async Task<int> Delete<T>(IUnitOfWork unitOfWork, Expression<Func<T, bool>> whereLambda, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = unitOfWork.DbConnection;
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            sql._Sql = new StringBuilder(LambdaToSqlHelper<T>.GetWhereByLambda(whereLambda, unitOfWork.DbType));
            var result = await db.ExecuteAsync(sql.DeleteSql, sql.Param);
            stopwatch.Stop();
            return result;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<int> DeleteById<T>(IUnitOfWork unitOfWork, object id = null, SqlQuery sql = null) where T : class
        {
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            var db = unitOfWork.DbConnection;
            var result = await db.ExecuteAsync(sql.DeleteSqlById, new { id });
            return result;
        }

        /// <summary>
        /// 删除 多个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static async Task<int> DeleteByIds<T>(IUnitOfWork unitOfWork, string ids, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            var db = unitOfWork.DbConnection;
            var result = await db.ExecuteAsync(sql.DeleteSqlByIds.Replace("@id", ids.TrimEnd(',').SqlRemoveStr()));
            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static async Task<int> Update<T>(IUnitOfWork unitOfWork, T t, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = unitOfWork.DbConnection;
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            sql = sql.AppendParam(t);
            var result = await db.ExecuteAsync(sql.UpdateSql, sql.Param);
            stopwatch.Stop();
            return result;
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<T> SingleOrDefault<T>(IUnitOfWork unitOfWork, object id, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = unitOfWork.DbConnection;
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            sql = sql.Top(1);
            var result = await db.QueryFirstOrDefaultAsync<T>(sql.QuerySqlById, new { id });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="whereLambda"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static async Task<T> SingleOrDefault<T>(IUnitOfWork unitOfWork, Expression<Func<T, bool>> whereLambda, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = unitOfWork.DbConnection;
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            sql._Sql = new StringBuilder(LambdaToSqlHelper<T>.GetWhereByLambda(whereLambda, unitOfWork.DbType));
            var result = await db.QueryFirstOrDefaultAsync<T>(sql.QuerySql);
            return result;
        }

        /// <summary>
        /// 查询 list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static async Task<List<T>> Query<T>(IUnitOfWork unitOfWork, Expression<Func<T, bool>> whereLambda, SqlQuery sql = null) where T : class
        {
            var db = unitOfWork.DbConnection;
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            sql._Sql = new StringBuilder(LambdaToSqlHelper<T>.GetWhereByLambda(whereLambda, unitOfWork.DbType));
            var result = await db.QueryAsync<T>(sql.QuerySql, sql.Param);
            return result.ToList();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="queryModel"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static async Task<PageData<T>> PageQuery<T>(IUnitOfWork unitOfWork, QueryModel queryModel,
           SqlQuery sqlQuery = null) where T : class
        {
            var db = unitOfWork.DbConnection;
            if (sqlQuery == null)
                sqlQuery = SqlQuery<T>.Builder(unitOfWork);
            queryModel.Items = queryModel.Items.Where(p => p.Value.ToString() != "").ToList();
            var where = "";
            if (queryModel.Items.Count() > 0)
            {
                where += SearchFilterHelper.ConvertFilters(queryModel.Items);
            }
            sqlQuery._Sql = new StringBuilder(where);
            sqlQuery = sqlQuery.Page(queryModel.PageIndex, queryModel.PageSize);
            var para = sqlQuery.Param;
            var cr = await db.QueryFirstOrDefault(sqlQuery.CountSql, para);
            var result = await db.QueryAsync<T>(sqlQuery.PageSql, para);
            return new PageData<T>()
            {
                DataList = result.ToList(),
                Total = cr.DataCount
            };
        }

        /// <summary>
        /// 查询条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static async Task<int> Count<T>(IUnitOfWork unitOfWork, SqlQuery sql = null) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var db = unitOfWork.DbConnection;
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            var result = await db.QueryFirstAsync(sql.CountSql, sql.Param);
            stopwatch.Stop();
            return result.DataCount;
        }


        public static async Task<DataTable> QueryDataTable<T>(IUnitOfWork unitOfWork, Expression<Func<T, bool>> whereLambda, SqlQuery sql = null) where T : class
        {
            var db = unitOfWork.DbConnection;
            if (sql == null)
                sql = SqlQuery<T>.Builder(unitOfWork);
            sql._Sql = new StringBuilder(LambdaToSqlHelper<T>.GetWhereByLambda(whereLambda, unitOfWork.DbType));
            DataTable dataTable = new DataTable();
            var result = await unitOfWork.DbConnection.ExecuteReaderAsync(sql.QuerySql, sql.Param);
            dataTable.Load(result);
            return dataTable;
        }
        #endregion

        #region  sql 语句执行
        /// <summary>
        /// sql 增删修
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static async Task<int> InsertUpdateOrDelete(IUnitOfWork unitOfWork, string sql, dynamic parms = null)
        {
            var result = await unitOfWork.DbConnection.ExecuteAsync(sql, (object)parms);
            return result;
        }

        /// <summary>
        /// sql 查询list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static async Task<List<T>> Query<T>(IUnitOfWork unitOfWork, string sql, dynamic parms = null)
        {
            var result = await unitOfWork.DbConnection.QueryAsync<T>(sql, (object)parms);
            return result.ToList();
        }

        public static async Task<PageData<T>> PageQuery<T>(IUnitOfWork unitOfWork, string sql, string countSql = "", dynamic parms = null)
        {
            var db = unitOfWork.DbConnection;
            var result = await db.QueryAsync<T>(sql, (object)parms);
            if (string.IsNullOrEmpty(countSql))
            {
                countSql = string.Format(" select count(*) as DataCount from {0} ", sql);
            }
            var cr = await db.QueryFirstOrDefaultAsync<dynamic>(countSql, (object)parms);
            return new PageData<T>()
            {
                DataList = result.ToList(),
                Total = cr.DataCount
            };
        }

        /// <summary>
        /// datatable
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static async Task<DataTable> QueryDataTable(IUnitOfWork unitOfWork, string sql, dynamic parms = null)
        {
            DataTable dataTable = new DataTable();
            var result = await unitOfWork.DbConnection.ExecuteReaderAsync(sql, (object)parms);
            dataTable.Load(result);
            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static async Task<T> SingleOrDefault<T>(IUnitOfWork unitOfWork, string sql, dynamic parms = null)
        {
            var result = await unitOfWork.DbConnection.QueryFirstOrDefaultAsync<T>(sql, (object)parms);
            return result;
        }


        /// <summary>
        /// 查询  一对一 或是一对多
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="countSql"></param>
        /// <param name="types"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static async Task<PageData<T>> PageQueryMult<T>(IUnitOfWork unitOfWork, Type[] types, Func<object[], T> map, string sql, string countSql = "", object param = null, string splitOn = "Id") where T : class
        {
            var db = unitOfWork.DbConnection;
            int total = 0;
            var result = await db.QueryAsync<T>(sql, types, map, param, null, true, splitOn);
            if (string.IsNullOrEmpty(countSql))
            {
                countSql = string.Format("select count(*) as DataCount from {0} ", sql);
            }
            var cr = await db.QueryFirstOrDefaultAsync(countSql);
            total = cr.DataCount;
            return new PageData<T>()
            {
                DataList = result.ToList(),
                Total = total
            };
        }

        #endregion

        #region 存储过程

        /// <summary>
        /// 执行存储过程 增修删
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static async Task<int> InsertUpdateOrDelete(IUnitOfWork unitOfWork, string procName, dynamic parms = null, CommandType? commandType = null)
        {
            var result = await unitOfWork.DbConnection.ExecuteAsync(procName, (object)parms, commandType: commandType);
            return result;
        }

        /// <summary>
        /// 存储过程的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<List<T>> Query<T>(IUnitOfWork unitOfWork, string procName, dynamic parms = null, CommandType? commandType = null)
        {
            var result = await unitOfWork.DbConnection.QueryAsync<T>(procName, (object)parms, commandType: commandType);
            return result.ToList();
        }

        /// <summary>
        /// 存储过程查询一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static async Task<T> SingleOrDefault<T>(IUnitOfWork unitOfWork, string procName, dynamic parms = null, CommandType? commandType = null)
        {
            var result = await unitOfWork.DbConnection.QueryFirstOrDefaultAsync<T>(procName, (object)parms, commandType: commandType);
            return result;
        }

        #endregion

    }
}
