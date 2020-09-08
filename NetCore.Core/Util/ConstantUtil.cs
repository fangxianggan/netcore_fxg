using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.Util
{
    public static class ConstantUtil
    {
        /// <summary>
        /// 请求的唯一标识
        /// </summary>
        public static string UUID = "ID";
        /// <summary>
        /// 请求url RequestUrl
        /// </summary>
        public static string REQUESTURL = "ApiUrl";
        /// <summary>
        /// 请求参数 RequestParameters
        /// </summary>
        public static string REQUESTPARAMETERS = "RequestParams";
        /// <summary>
        /// Headers（可以包含：Authorization授权认证）
        /// </summary>
        public static string HEADERS = "RequestHead";
        /// <summary>
        /// 是否发送邮件
        /// </summary>
        public static string MAILMESSAGE = "MailMessage";
        /// <summary>
        /// 请求类型 RequestType
        /// </summary>
        public static string REQUESTTYPE = "RequestType";
        /// <summary>
        /// 日志 LogList
        /// </summary>
        public static string LOGLIST = "LogList";
        /// <summary>
        /// 异常 Exception
        /// </summary>
        public static string EXCEPTION = "Exception";
    }
}
