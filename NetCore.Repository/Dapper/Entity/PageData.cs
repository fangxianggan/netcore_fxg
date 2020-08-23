using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Repository.Dapper.Entity
{
   public partial class PageData<T>
    {

        public List<T> DataList { set; get; }

        public int Total { set; get; }
    }
}
