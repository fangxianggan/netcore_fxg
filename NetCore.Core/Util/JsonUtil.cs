using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.Util
{
    public static class JsonUtil
    {
        /// <summary>
        /// 将对象进行序列化。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";


        //序列化  
        public static string JsonSerialize(object o)
        {
            if (o == null || o.ToString() == "null") return null;
            if (o != null && (o.GetType() == typeof(String) || o.GetType() == typeof(string)))
            {
                return o.ToString();
            }

            IsoDateTimeConverter dt = new IsoDateTimeConverter();
            dt.DateTimeFormat = DateTimeFormat;
            return JsonConvert.SerializeObject(o, dt);
        }


       
        //反序列化  
        public static T JsonDeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }




    }
}
