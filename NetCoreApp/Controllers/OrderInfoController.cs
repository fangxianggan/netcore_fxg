using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.RequestViewModel.ProductInfo;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderInfoController : ControllerBase
    {

        /// <summary>
        /// 购买提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("SumbitOrderData")]
        public async Task<HttpReponseViewModel> SumbitOrderData(QGProductViewModel model)
        {
            return await Task.Run(()=> {

                return "";
            });
        }

    }
}