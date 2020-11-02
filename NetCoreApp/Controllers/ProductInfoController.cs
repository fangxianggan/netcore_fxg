using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.Enum;
using NetCore.DTO.ReponseViewModel.ProductInfo;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_ProductInfo;
using NetCore.Services.IServices.I_Redis;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 产品
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductInfoController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductInfoServices _productInfoServices;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisServices"></param>
        /// <param name="productInfoServices"></param>
        public ProductInfoController(IRedisServices redisServices, IProductInfoServices productInfoServices)
        {
            _productInfoServices = productInfoServices;

        }


        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("GetPageList")]
        public async Task<HttpReponsePageViewModel<List<ProductInfoViewModel>>> GetPageList(QueryModel queryModel)
        {
            return await Task.Run(() =>
            {
                return _productInfoServices.GetPageListService(queryModel);
            });

        }

        /// <summary>
        /// 新增修改产品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost, Route("AddOrEdit")]
        public async Task<HttpReponseObjViewModel<string>> AddOrEdit(ProductInfoViewModel model)
        {
            return await Task.Run(() =>
            {
                return _productInfoServices.AddOrEditService(model, true, EmDataKey.ProductInfoHash.ToString());
            });
        }

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        ///

        [HttpPost, Route("Delete")]
        public async Task<HttpReponseViewModel> Delete([FromBody] Guid gId)
        {
            return await _productInfoServices.DeleteService(gId,true, EmDataKey.ProductInfoHash.ToString());
        }


    }
}