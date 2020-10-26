using Microsoft.AspNetCore.Http;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.EntityModel.ReponseModels
{

    public class HttpReponseViewModel
    {
        /// <summary>
        /// 返回标记
        /// </summary>
        public ResultSign ResultSign { get; set; }

        /// <summary>
        /// 消息字符串(有多语言后将删除该属性)
        /// </summary>
        public string Message { get; set; }


        public string ExeSql { set; get; }

        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCode StatusCode { set; get; }

        public HttpReponseViewModel(string exeSql = "")
        {
            ResultSign = ResultSign.Success;
            Message = HttpReponseMessage.SuccessMsg;
            StatusCode = StatusCode.OK;
            ExeSql = exeSql;
        }

        public HttpReponseViewModel(bool flag, string exeSql = "")
        {
            if (!flag)
            {
                ResultSign = ResultSign.Error;
                Message = HttpReponseMessage.ErrorMsg;
                StatusCode = StatusCode.InternalServerError;
            }
            ExeSql = exeSql;
        }
    }


    public class HttpReponsePageViewModel<T> : HttpReponseViewModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 每页个数
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 总和
        /// </summary>
        public long Total { set; get; }
        /// <summary>
        /// 分页数据
        /// </summary>
        public T PageData { get; set; }


        public HttpReponsePageViewModel()
        {
            PageIndex = 1;
            PageSize = 20;
            Total = 0;
            PageData = default(T);
        }

        public HttpReponsePageViewModel( T t = default(T), int pageIndex = 1, int pageSize = 20, long total = 0, string exeSql = "")
        {
            
            ExeSql = exeSql;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            PageData = t;
        }


    }


    public class HttpReponseObjViewModel<T> : HttpReponseViewModel
    {
        /// <summary>
        /// 对象
        /// </summary>
        public T Data { set; get; }

        public HttpReponseObjViewModel()
        {
            Data = default(T);
        }

        public HttpReponseObjViewModel(bool flag, string exeSql = "")
        {
            if (!flag)
            {
                ResultSign = ResultSign.Error;
                Message = HttpReponseMessage.ErrorMsg;
                StatusCode = StatusCode.InternalServerError;
            }
            Data = default(T);
            ExeSql = exeSql;
        }

    }


}
