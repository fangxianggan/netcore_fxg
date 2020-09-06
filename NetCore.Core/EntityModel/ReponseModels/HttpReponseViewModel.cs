using Microsoft.AspNetCore.Http;
using NetCore.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.EntityModel.ReponseModels
{
    /// <summary>
    /// httpreponse
    /// </summary>
    public class HttpReponseViewModel
    {
        public HttpReponseViewModel()
        {
            ResultSign = ResultSign.Success;
            Message = HttpReponseMessageViewModel.SuccessMsg;
            Code = StatusCodes.Status200OK;
            RequestParams = null;
            Flag = true;
            EXESql = "";
            PrimaryKeyValue = "";
        }
        public HttpReponseViewModel(bool flag,string primaryKeyValue="", string errorMsg = "", string successMsg = "", string exesql = "")
        {
            if (flag)
            {
                ResultSign = ResultSign.Success;
                if (string.IsNullOrEmpty(successMsg))
                {
                    successMsg = HttpReponseMessageViewModel.SuccessMsg;
                }
                Message = successMsg;
                Code = StatusCodes.Status200OK;
            }
            else
            {
                ResultSign = ResultSign.Error;
                if (string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = HttpReponseMessageViewModel.ErrorMsg;
                }
                Message = errorMsg;
                Code = StatusCodes.Status500InternalServerError;
            }
            Flag = flag;
            RequestParams = null;
            EXESql = exesql;
            PrimaryKeyValue = primaryKeyValue;
        }


        #region 属性
        /// <summary>
        ///     返回标记
        /// </summary>
        public ResultSign ResultSign { get; set; }

        /// <summary>
        ///     消息字符串(有多语言后将删除该属性)
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  传入的参数
        /// </summary>
        public object RequestParams { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { set; get; }

        /// <summary>
        /// 判断执行的是不是成功
        /// </summary>
        public bool Flag { set; get; }

        /// <summary>
        /// token
        /// </summary>
        public string Token { set; get; }

        /// <summary>
        /// 执行的sql
        /// </summary>
        public string EXESql { set; get; }


        /// <summary>
        /// 返回主键值 （主要用于前端页面不刷新 需要主键值）
        /// </summary>
        public string PrimaryKeyValue { set; get; }
        #endregion
    }

    /// <summary>
    /// 返回实体对象 包含分页功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpReponseViewModel<T> : HttpReponseViewModel
    {
        public HttpReponseViewModel()
        {
            PageIndex = 0;
            PageSize = 0;
            Total = 0;
            Data = default(T);
        }
        public HttpReponseViewModel(bool flag, T t = default(T), int? pageIndex = 1, int? pageSize = 20, long? total = 0, string exeSql = "")
        {
            if (flag)
            {
                ResultSign = ResultSign.Success;
                Message = HttpReponseMessageViewModel.SuccessMsg;
                Code = StatusCodes.Status200OK;
            }
            else
            {
                ResultSign = ResultSign.Error;
                Message = HttpReponseMessageViewModel.ErrorMsg;
                Code = StatusCodes.Status500InternalServerError;
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Data = t;
            EXESql = exeSql;
        }

        public HttpReponseViewModel(T t, int? pageIndex = 1, int? pageSize = 20, long? total = 0, string exeSql = "")
        {
            if (t != null)
            {
                ResultSign = ResultSign.Success;
                Message = HttpReponseMessageViewModel.SuccessMsg;
                Code = StatusCodes.Status200OK;
            }
            else {
                ResultSign = ResultSign.Error;
                Message = HttpReponseMessageViewModel.ErrorMsg;
                Code = StatusCodes.Status500InternalServerError;
            }
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Data = t;
            EXESql = exeSql;
        }
        public int? PageIndex { set; get; }

        public int? PageSize { set; get; }

        public long? Total { set; get; }
        /// <summary>
        /// 泛型对象
        /// </summary>
        public T Data { get; set; }

    }
}
