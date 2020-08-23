using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Util
{
    public static class WebApiUtil
    {

        public static string Get(string url, ContentType contentType)
        {
            string result = "";
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var myHttpClient = new HttpClient(handler))
            {
               // myHttpClient.BaseAddress = new Uri(url);
                myHttpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                //GET提交 返回string
                HttpResponseMessage response = myHttpClient.GetAsync(url).Result;
            
                if (contentType == ContentType.text)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        result = "error";
                    }
                }
                else if (contentType == ContentType.json)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        responseContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        result = responseContent.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        result = "{\"error\"}";
                    }
                }

            }
            return result;
        }


        public static string Post(string url, object o, ContentType contentType)
        {
            //string result = "";
            ////创建HttpClient（注意传入HttpClientHandler）(主要用户网页中的压缩文件 解压问题 暂时用不到)
            //var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            //using (var http = new HttpClient(handler))
            //{
            //    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            //    Type type = o.GetType();
            //    PropertyInfo[] ps = type.GetProperties();
            //    foreach (PropertyInfo item in ps)
            //    {
            //        paramList.Add(new KeyValuePair<string, string>(item.Name, item.GetValue(o).ToString()));
            //    }
            //    http.DefaultRequestHeaders.Add("KeepAlive", "false");
            //    var response = http.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
            //    if (contentType == ContentType.text)
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            result = response.Content.ReadAsStringAsync().Result;
            //        }
            //        else
            //        {
            //            result = "error";
            //        }
            //    }
            //    else if (contentType == ContentType.json)
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var responseContent = response.Content;
            //            responseContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            //            result = responseContent.ReadAsStringAsync().Result;
            //        }
            //        else
            //        {
            //            result = "{\"error\"}";
            //        }
            //    }
            //    return result;
            //}

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
           
            var requestJson = JsonConvert.SerializeObject(o);
            HttpContent httpcontent = new StringContent(requestJson);
            httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = httpClient.PostAsync(url, httpcontent).Result.Content.ReadAsStringAsync().Result;
            return result;
        }
    }

    /// <summary>
    /// 枚举
    /// </summary>
    public enum ContentType
    {

        [Description("text/html")]
        html,
        [Description("text/plain")]
        text,
        [Description("text/xml")]
        xml,
        [Description("image/gif")]
        gif,
        [Description("image/jpg")]
        jpg,
        [Description("image/png")]
        png,
        [Description("application/json")]
        json,
        [Description("application/octet-stream")]
        stream,
        [Description("application/pdf")]
        pdf
    }


}
