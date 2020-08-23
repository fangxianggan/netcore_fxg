using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Util
{

    public  static class DisplayNameUtil
    {
        public static Dictionary<string, string> DisplayNameModel<T>(T t)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var p in properties)
            {
                var name = p.Name;
                //display名字
                var displayName = p.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                dic.Add(name, displayName);
            }
            return dic;
        }
    }


}
