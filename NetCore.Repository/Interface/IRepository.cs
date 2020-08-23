using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Repository.Interface
{
    public interface IRepository<T> : IDapperRepository<T> where T : class
    {

    }
}
