using NetCore.Core.Enum;
using System;
using System.Data;

namespace NetCore.Repository.Interface
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Guid ID { get; }
        string ParamPrefix { get; }

        DBType DbType { get; }

        /// <summary>
        /// 事务
        /// </summary>
        IDbTransaction DbTransaction { get; }
        /// <summary>
        /// 数据连接
        /// </summary>
        IDbConnection DbConnection { get; }

        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 完成事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
    }

}
