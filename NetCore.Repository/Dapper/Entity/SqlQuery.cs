
using NetCore.Core.Enum;
using NetCore.EntityModel.QueryModels;
using NetCore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NetCore.Repository.Dapper.Entity
{
    public abstract class SqlQuery
    {
        private static object objLock = new object();
        protected int _TopNum; //查询TOP
        protected QueryOrder _Order;  //排序
        public StringBuilder _Sql; //组装的SQL WHERE部分
        protected IList<DynamicPropertyModel> _Param;  //参数动态类
        protected string ParamPrefix = "@"; //参数前缀
        public ModelDes _ModelDes;//处理的实体对象描述
        protected int _PageIndex;
        protected int _PageSize;
        protected DBType DbType;
        protected IList<string> ExcColums;//不加入SQL执行的列

        //动态参数类缓存
        protected static Dictionary<string, Type> DynamicParamModelCache = new Dictionary<string, Type>();

        /// <summary>
        /// 记录参数名和值
        /// </summary>
        protected Dictionary<string, object> ParamValues = new Dictionary<string, object>();
        /// <summary>
        /// TOP
        /// </summary>
        public virtual int TopNum { get { return _TopNum; } }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual QueryOrder Order { get { return _Order; } }

        /// <summary>
        /// SQL字符串,只表示包括Where部分
        /// </summary>
        public virtual string WhereSql
        {
            get
            {
                var sb = new StringBuilder();
                var arr = _Sql.ToString().Split(' ').Where(m => !string.IsNullOrEmpty(m)).ToList();
                if (arr.Count > 0)
                {
                    sb.Append("WHERE");
                }
                for (int i = 0; i < arr.Count; i++)
                {
                    if (i == 0 && (arr[i] == "AND" || arr[i] == "OR"))
                    {
                        continue;
                    }
                    if (i > 0 && arr[i - 1] == "(" && (arr[i] == "AND" || arr[i] == "OR"))
                    {
                        continue;
                    }
                    sb.Append(" ");
                    sb.Append(arr[i]);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual string OrderSql
        {
            get
            {
                var sb = new StringBuilder();
                if (Order != null)
                {
                    sb.Append(" ");
                    sb.Append("ORDER BY");
                    sb.Append(" ");
                    sb.Append(Order.Field);
                    sb.Append(" ");
                    var desc = Order.IsDesc ? "DESC" : "ASC";
                    sb.Append(desc);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 查询参数对象
        /// </summary>
        public object Param
        {
            get
            {
                if (_Param == null || _Param.Count <= 0) return null;

                #region 处理参数对象是否存在缓存中
                var paramKeys = ParamValues.Keys.ToList();//当前使用的参数集合
                var listCacheKeys = new List<string> { _ModelDes.TableName };
                listCacheKeys.AddRange(paramKeys);

                var cacheKey = string.Empty;
                foreach (var key in DynamicParamModelCache.Keys.Where(m => m.StartsWith(_ModelDes.TableName)).Where(key => listCacheKeys.All(m => key.Split('_').Contains(m))))
                {
                    cacheKey = key;
                    break;
                }
                if (string.IsNullOrEmpty(cacheKey))//为空则说明缓存不存在相应数据类型
                {
                    cacheKey = string.Join("_", listCacheKeys);
                }
                #endregion
                Type modelType;
                lock (objLock)//防止多线程同时操作DynamicParamModelCache
                {
                    DynamicParamModelCache.TryGetValue(cacheKey, out modelType);
                    if (modelType == null)
                    {
                        const string tyName = "CustomDynamicParamClass";
                        modelType = CustomDynamicBuilder.DynamicCreateType(tyName, _Param);
                        DynamicParamModelCache.Add(cacheKey, modelType);
                    }
                }
                var model = Activator.CreateInstance(modelType);
                foreach (var item in ParamValues)
                {
                    modelType.GetProperty(item.Key).SetValue(model, item.Value, null);
                }
                return model;
            }
        }
        /// <summary>
        /// 插入语句SQL
        /// </summary>
        public virtual string InsertSql
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(string.Format("INSERT INTO {0}(", _ModelDes.TableName));
                var colums = DapperCacheCommon.GetExecColumns(_ModelDes);

                for (int i = 0; i < colums.Count; i++)
                {
                    sql.Append(i == 0 ? colums[i].ColumnName : string.Format(",{0}", colums[i].ColumnName));
                }
                sql.Append(")");
                sql.Append(" VALUES(");
                for (int i = 0; i < colums.Count; i++)
                {
                    sql.Append(i == 0
                        ? string.Format("{0}{1}", ParamPrefix, colums[i].FieldName)
                        : string.Format(",{0}{1}", ParamPrefix, colums[i].FieldName));
                }
                sql.Append(") ");
                return sql.ToString();
            }
        }
        /// <summary>
        /// 删除SQL
        /// </summary>
        public virtual string DeleteSql
        {
            get
            {
                return string.Format("DELETE FROM {0} {1}", _ModelDes.TableName, WhereSql);
            }
        }

        /// <summary>
        /// 删除SQL
        /// </summary>
        public virtual string DeleteSqlById
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                if (string.IsNullOrEmpty(WhereSql))//没有where条件的情况
                {
                    var p = DapperCacheCommon.GetPrimary(_ModelDes);
                    sql.Append(string.Format(" WHERE {0}={1}", p.Column, ParamPrefix + "id"));
                }
                else
                {
                    sql.Append(string.Format(" {0}", WhereSql));
                }
                return string.Format("DELETE FROM {0} {1}", _ModelDes.TableName, sql);
            }
        }

        /// <summary>
        /// 删除SQL
        /// </summary>
        public virtual string DeleteSqlByIds
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                if (string.IsNullOrEmpty(WhereSql))//没有where条件的情况
                {
                    var p = DapperCacheCommon.GetPrimary(_ModelDes);
                    sql.Append(string.Format(" WHERE {0} in ({1})", p.Column, ParamPrefix + "id"));
                }
                else
                {
                    sql.Append(string.Format(" {0}", WhereSql));
                }
                return string.Format("DELETE FROM {0} {1}", _ModelDes.TableName, sql);
            }
        }

        /// <summary>
        /// 修改SQL
        /// </summary>
        public virtual string UpdateSql
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(string.Format("UPDATE {0} SET", _ModelDes.TableName));
                var colums = DapperCacheCommon.GetExecColumns(_ModelDes, false);
                if (ExcColums != null && ExcColums.Count > 0)
                {
                    colums = colums.Where(m => ExcColums.Contains(m.ColumnName)).ToList();
                }
                for (int i = 0; i < colums.Count; i++)
                {
                    if (i != 0) sql.Append(",");
                    sql.Append(" ");
                    sql.Append(colums[i].ColumnName);
                    sql.Append(" ");
                    sql.Append("=");
                    sql.Append(" ");
                    sql.Append(ParamPrefix + colums[i].FieldName);
                }
                if (string.IsNullOrEmpty(WhereSql))//没有where条件的情况
                {
                    var p = DapperCacheCommon.GetPrimary(_ModelDes);
                    sql.Append(string.Format(" WHERE {0}={1}", p.Column, ParamPrefix + p.Field));
                }
                else
                {
                    sql.Append(string.Format(" {0}", WhereSql));
                }

                return sql.ToString();
            }
        }
        /// <summary>
        /// 查询SQL
        /// </summary>
        public virtual string QuerySql
        {
            get
            {
                var sqlStr = "";
                if (_TopNum > 0)
                {
                    if (DbType == DBType.SqlServer || DbType == DBType.SqlServerCE)
                        sqlStr = string.Format("SELECT TOP {0} {1} FROM {2} {3} {4}", _TopNum, "*", _ModelDes.TableName, WhereSql, OrderSql);
                    else if (DbType == DBType.Oracle)
                    {
                        var strWhere = "";
                        strWhere = string.IsNullOrEmpty(WhereSql) ? string.Format(" WHERE  ROWNUM <= {0} ", _TopNum) : string.Format(" {0} AND ROWNUM <= {1} ", WhereSql, _TopNum);
                        sqlStr = string.Format("SELECT * FROM {0} {1} {2}", _ModelDes.TableName, strWhere, OrderSql);
                    }
                    else
                    {
                        sqlStr = string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4}", "*", _ModelDes.TableName, WhereSql, OrderSql, _TopNum);
                    }
                }
                else
                {
                    sqlStr = string.Format("SELECT {0} FROM {1} {2} {3}", "*", _ModelDes.TableName, WhereSql, OrderSql);
                }
                return sqlStr;
            }
        }

        /// <summary>
        /// 查询多个
        /// </summary>
        public virtual string QuerySqlByIds
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                if (string.IsNullOrEmpty(WhereSql))//没有where条件的情况
                {
                    var p = DapperCacheCommon.GetPrimary(_ModelDes);
                    sql.Append(string.Format(" WHERE {0} in ({1})", p.Column, ParamPrefix + "id"));
                }
                else
                {
                    sql.Append(string.Format(" {0}", WhereSql));
                }
                return string.Format("SELECT {0} FROM {1} {2} {3}", "*", _ModelDes.TableName, sql, OrderSql);
            }
        }

        /// <summary>
        /// 查询SQL
        /// </summary>
        public virtual string QuerySqlById
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                if (string.IsNullOrEmpty(WhereSql))//没有where条件的情况
                {
                    var p = DapperCacheCommon.GetPrimary(_ModelDes);
                    sql.Append(string.Format(" WHERE {0}={1}", p.Column, ParamPrefix + "id"));
                }
                else
                {
                    sql.Append(string.Format(" {0}", WhereSql));
                }

                var sqlStr = "";
                if (_TopNum > 0)
                {
                    if (DbType == DBType.SqlServer || DbType == DBType.SqlServerCE)
                        sqlStr = string.Format("SELECT TOP {0} {1} FROM {2} {3} {4}", _TopNum, "*", _ModelDes.TableName, sql, OrderSql);
                    else if (DbType == DBType.Oracle)
                    {
                        var strWhere = "";
                        strWhere = string.IsNullOrEmpty(WhereSql) ? string.Format(" WHERE  ROWNUM <= {0} ", _TopNum) : string.Format(" {0} AND ROWNUM <= {1} ", sql, _TopNum);
                        sqlStr = string.Format("SELECT * FROM {0} {1} {2}", _ModelDes.TableName, strWhere, OrderSql);
                    }
                    else
                    {
                        sqlStr = string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4}", "*", _ModelDes.TableName, sql, OrderSql, _TopNum);
                    }
                }
                else
                {
                    sqlStr = string.Format("SELECT {0} FROM {1} {2} {3}", "*", _ModelDes.TableName, sql, OrderSql);
                }
                return sqlStr;
            }
        }

        /// <summary>
        /// 分页SQL
        /// </summary>
        public virtual string PageSql
        {
            get
            {
                var sqlPage = "";
                var firstOrDefault = _ModelDes.Properties.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    var orderStr = string.IsNullOrEmpty(OrderSql) ? "ORDER BY " + firstOrDefault.Column : OrderSql;

                    if (DbType == DBType.SqlServer || DbType == DBType.Oracle)
                    {
                        var tP = DbType == DBType.Oracle ? _ModelDes.TableName + ".*" : "*";
                        sqlPage = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) rid, {1} FROM {5} {2} ) p_paged WHERE rid>{3} AND rid<={4}",
                            orderStr, tP, WhereSql, (_PageIndex - 1) * _PageSize, (_PageIndex - 1) * _PageSize + _PageSize, _ModelDes.TableName);
                    }
                    else if (DbType == DBType.SqlServerCE)
                    {
                        sqlPage = string.Format("SELECT * FROM {0} {1} {2} OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY",
                            _ModelDes.TableName, WhereSql, orderStr, (_PageIndex - 1) * _PageSize, (_PageIndex - 1) * _PageSize + _PageSize);
                    }
                    else
                    {
                        sqlPage = string.Format("SELECT * FROM {0} {1} {2} LIMIT {1} OFFSET {2}",
                            _ModelDes.TableName, WhereSql, orderStr, (_PageIndex - 1) * _PageSize, (_PageIndex - 1) * _PageSize + _PageSize);
                    }
                }
                return sqlPage;
            }
        }
        /// <summary>
        /// 数据总是SQL
        /// </summary>
        public virtual string CountSql
        {
            get
            {
                return string.Format("SELECT COUNT(*) DataCount FROM {0} {1}", _ModelDes.TableName, WhereSql);
            }
        }

        protected SqlQuery()
        {
            _Sql = new StringBuilder();
        }
        /// <summary>
        /// TOP
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public SqlQuery Top(int top)
        {
            _TopNum = top;
            return this;
        }
        /// <summary>
        /// 奖其它参数添加到参数对象中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public SqlQuery AppendParam<T>(T t) where T : class
        {
            if (_Param == null)
            {
                _Param = new List<DynamicPropertyModel>();
            }
            var model = DapperCacheCommon.GetModelDes<T>();
            foreach (var item in model.Properties)
            {
                var value = model.ClassType.GetProperty(item.Field).GetValue(t, null);
                ParamValues.Add(item.Field, value);
                var pmodel = new DynamicPropertyModel
                {
                    Name = item.Field,
                    PropertyType = value != null ? value.GetType() : typeof(String)
                };
                _Param.Add(pmodel);
            }
            return this;
        }
        /// <summary>
        /// 分页条件
        /// </summary>
        /// <param name="pindex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public SqlQuery Page(int pageIndex, int pageSize)
        {
            _PageIndex = pageIndex;
            _PageSize = pageSize;
            return this;
        }
        /// <summary>
        /// 设置执行的属性列
        /// </summary>
        /// <param name="properties">属性集合</param>
        /// <returns></returns>
        public SqlQuery SetExcProperties<T>(IList<string> properties)
        {
            if (ExcColums == null)
                ExcColums = new List<string>();
            foreach (var col in properties.Select(item => _ModelDes.Properties.FirstOrDefault(m => m.Field == item)).Where(col => col != null && (!ExcColums.Contains(col.Column))))
            {
                ExcColums.Add(col.Column);
            }
            return this;
        }
    }
    /// <summary>
    ///  组装查询
    /// </summary>
    public class SqlQuery<T> : SqlQuery where T : class
    {
       
        private SqlQuery()
        {
            _ModelDes = DapperCacheCommon.GetModelDes<T>();
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public static SqlQuery<T> Builder(IUnitOfWork unitOfWork)
        {
            var result = new SqlQuery<T>
            {
                ParamPrefix = unitOfWork.ParamPrefix,
                DbType = unitOfWork.DbType
            };
            return result;
        }
        /// <summary>
        /// 设置不执行的属性列
        /// </summary>
        /// <param name="properties">属性集合</param>
        /// <returns></returns>
        public  SqlQuery<T> SetExcProperties(IList<string> properties)
        {
            if (ExcColums == null)
                ExcColums = new List<string>();
            foreach (var item in properties)
            {
                var col = _ModelDes.Properties.FirstOrDefault(m => m.Field == item);
                if (col != null && (!ExcColums.Contains(col.Column)))
                    ExcColums.Add(col.Column);
            }
            return this;
        }
        /// <summary>
        /// 创建排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public  SqlQuery<T> OrderBy(Expression<Func<T, object>> expr, bool desc)
        {
            var field = DapperCacheCommon.GetPropertyByExpress(_ModelDes, expr).Column;
            _Order = new QueryOrder() { Field = field, IsDesc = desc };
            return this;
        }
        /// <summary>
        /// TOP
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public new SqlQuery<T> Top(int top)
        {
            _TopNum = top;
            return this;
        }
        /// <summary>
        /// 左括号(
        /// </summary>
        /// <param name="isAnd">true为AND false为OR</param>
        /// <returns></returns>
        public SqlQuery<T> LeftInclude(bool isAnd = true)
        {
            var cn = isAnd ? "AND" : "OR";
            _Sql.Append(" ");
            _Sql.Append(cn);
            _Sql.Append(" ");
            _Sql.Append("(");
            return this;
        }
        /// <summary>
        /// 右括号)
        /// </summary>
        /// <returns></returns>
        public SqlQuery<T> RightInclude()
        {
            _Sql.Append(" ");
            _Sql.Append(")");
            return this;
        }
        /// <summary>
        /// AND方式连接一条查询条件
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public  SqlQuery<T> AndWhere(Expression<Func<T, object>> expr, QueryMethod operation, object value)
        {
            return Where(expr, operation, value, true);
        }
        /// <summary>
        ///  Or方式连接一条查询条件
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlQuery<T> OrWhere(Expression<Func<T, object>> expr, Core.Enum.QueryMethod operation, object value)
        {
            return Where(expr, operation, value, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <param name="isAnd">true为AND false为OR</param>
        /// <returns></returns>
        private SqlQuery<T> Where(Expression<Func<T, object>> expr, QueryMethod operation, object value, bool isAnd)
        {
            var cn = isAnd ? "AND" : "OR";
            var field = DapperCacheCommon.GetPropertyByExpress(_ModelDes, expr).Column;
            var op = GetOpStr(operation);
            _Sql.Append(" ");
            _Sql.Append(cn);
            _Sql.Append(" ");
            _Sql.Append(field);
            _Sql.Append(" ");
            _Sql.Append(op);
            _Sql.Append(" ");
            var model = AddParam(operation, field, value);
            _Sql.Append(ParamPrefix + model.Name);
            return this;
        }
        /// <summary>
        /// 比较符
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private string GetOpStr(QueryMethod method)
        {
            switch (method)
            {
                case QueryMethod.Contains:
                    return "LIKE";
                case QueryMethod.EndsWith:
                    return "LIKE";
                case QueryMethod.Equal:
                    return "=";
                case QueryMethod.GreaterThan:
                    return ">";
                case QueryMethod.GreaterThanOrEqual:
                    return ">=";
                case QueryMethod.In:
                    return "IN";
                case QueryMethod.LessThan:
                    return "<";
                case QueryMethod.LessThanOrEqual:
                    return "<=";
                case QueryMethod.NotEqual:
                    return "<>";
                case QueryMethod.StartsWith:
                    return "LIKE";
            }
            return "=";
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="method"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private object CreateParam(QueryMethod method, object value)
        {
            switch (method)
            {
                case QueryMethod.Contains:
                    return string.Format("%{0}%", value);
                case QueryMethod.EndsWith:
                    return string.Format("%{0}", value);
                case QueryMethod.Equal:
                    return value;
                case QueryMethod.GreaterThan:
                    return value;
                case QueryMethod.GreaterThanOrEqual:
                    return value;
                case QueryMethod.In:
                    return value;
                case QueryMethod.LessThan:
                    return value;
                case QueryMethod.LessThanOrEqual:
                    return value;
                case QueryMethod.NotEqual:
                    return value;
                case QueryMethod.StartsWith:
                    return string.Format("{0}%", value);
            }
            return value;
        }

        /// <summary>
        /// 通过方法和值创建一个参数对象并记录
        /// </summary>
        /// <param name="method"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private DynamicPropertyModel AddParam(QueryMethod method, string field, object value)
        {
            if (_Param == null)
            {
                _Param = new List<DynamicPropertyModel>();
            }

            var model = new DynamicPropertyModel();
            model.Name = field + GetParamIndex(field);
            model.PropertyType = value.GetType();
            _Param.Add(model);

            switch (method)
            {
                #region
                case QueryMethod.Contains:
                    ParamValues.Add(model.Name, string.Format("%{0}%", value));
                    break;
                case QueryMethod.EndsWith:
                    ParamValues.Add(model.Name, string.Format("%{0}", value));
                    break;
                case QueryMethod.Equal:
                    ParamValues.Add(model.Name, value);
                    break;
                case QueryMethod.GreaterThan:
                    ParamValues.Add(model.Name, value);
                    break;
                case QueryMethod.GreaterThanOrEqual:
                    ParamValues.Add(model.Name, value);
                    break;
                case QueryMethod.In:
                    ParamValues.Add(model.Name, value);
                    break;
                case QueryMethod.LessThan:
                    ParamValues.Add(model.Name, value);
                    break;
                case QueryMethod.LessThanOrEqual:
                    ParamValues.Add(model.Name, value);
                    break;
                case QueryMethod.NotEqual:
                    ParamValues.Add(model.Name, value);
                    break;
                case QueryMethod.StartsWith:
                    ParamValues.Add(model.Name, string.Format("{0}%", value));
                    break;
                    #endregion
            }
            return model;
        }
        private string GetParamIndex(string field)
        {
            var key = ParamValues.Keys.FirstOrDefault(m => m.StartsWith(field));
            return key == null ? "1" : (int.Parse(key.Remove(0, field.Length)) + 1).ToString();
        }
    }
}
