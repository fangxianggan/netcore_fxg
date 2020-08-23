using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using NetCore.Core.Enum;
using NetCore.Repository.Interface;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NetCore.Repository.UnitWork
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private void SetParamPrefix()
        {
            string dbtype = (_dbFactory == null ? _connection.GetType() : _dbFactory.GetType()).Name;

            // 使用类型名判断
            if (dbtype.StartsWith("MySql")) _dbType = DBType.MySql;
            else if (dbtype.StartsWith("SqlCe")) _dbType = DBType.SqlServerCE;
            else if (dbtype.StartsWith("Npgsql")) _dbType = DBType.PostgreSQL;
            else if (dbtype.StartsWith("Oracle")) _dbType = DBType.Oracle;
            else if (dbtype.StartsWith("SQLite")) _dbType = DBType.SQLite;
            else if (dbtype.StartsWith("System.Data.SqlClient.")) _dbType = DBType.SqlServer;
            // else try with provider name
            else if (_providerName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.MySql;
            else if (_providerName.IndexOf("SqlServerCe", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SqlServerCE;
            else if (_providerName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.PostgreSQL;
            else if (_providerName.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.Oracle;
            else if (_providerName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SQLite;

            if (_dbType == DBType.MySql && _connection != null && _connection.ConnectionString != null && _connection.ConnectionString.IndexOf("Allow User Variables=true") >= 0)
                _paramPrefix = "?";
            if (_dbType == DBType.Oracle)
                _paramPrefix = ":";
        }



        private string _providerName = "System.Data.Odbc";
        private DbProviderFactory _dbFactory;
        private string _paramPrefix = "@";
        private DBType _dbType = DBType.MySql;
        private IDbConnection _connection;
        private string _connectionStringName;
        public string ParamPrefix
        {
            get
            {
                return _paramPrefix;
            }
        }

        public string ProviderName
        {
            get
            {
                return _providerName;
            }
        }
        public DBType DbType
        {
            get
            {
                return _dbType;
            }
        }
        public IDbConnection DbConnection
        {
            get
            {
                return  _connection;
            }
        }
        private Guid _id;
        public Guid ID { get { return _id; } }
        public UnitOfWork(IConfiguration Configuration)
        {
            //读取配置文件，数据库连接字符串
            var connectionString = Configuration.GetConnectionString("MySqlConnection");
            //运用 miniprofiler 检测
            _connection = new ProfiledDbConnection(new MySqlConnection(connectionString), MiniProfiler.Current);
            _connection.Open();
            _id = Guid.NewGuid();
        }

        private IDbTransaction _trans = null;

        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction DbTransaction { get { return _trans; } }

        private bool _disposed;


        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            _trans = _connection.BeginTransaction();
        }
        /// <summary>
        /// 完成事务
        /// </summary>
        public void Commit() => _trans?.Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback() => _trans?.Rollback();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _trans?.Dispose();
                _connection?.Dispose();
            }

            _trans = null;
            _connection = null;
            _disposed = true;
        }
    }
}
