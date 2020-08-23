using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetCore.Core.Extensions
{
    public static class DictionaryExtension
    {

        /// <summary>
        /// dic 字典里按照ascii排序
        /// </summary>
        /// <param name="paramsMap"></param>
        /// <returns></returns>
        public static string ToSortUrlParamString<TKey, TValue>(this Dictionary<TKey, TValue> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<TKey, TValue> kv in vDic)
            {
                var pkey = kv.Key;
                var pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }
            string result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }

       
        
    }
}
