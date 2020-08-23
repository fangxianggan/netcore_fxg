using NetCore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Repository.Repository
{
    public class Repository<T> : DapperRepository<T>, IRepository<T> where T : class, new()
    {

        private readonly IUnitOfWork _unitOfWork;
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
