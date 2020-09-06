using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Extensions
{
   public static  class ListExtension
    {
        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (T value in list)
            {
                await func(value);
            }
        }
    }
}
