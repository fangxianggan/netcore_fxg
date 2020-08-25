using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_StoreFiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]

    public class StoreFilesController : ControllerBase
    {
        private static IHostingEnvironment _hostingEnvironment;
        private readonly IFileUploadServices _uploadServices;
        private readonly IFileDownloadServices _downloadServices;
        private readonly IStoreFilesServices _storeFilesServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="uploadServices"></param>
        public StoreFilesController(IHostingEnvironment hostingEnvironment, IFileUploadServices uploadServices, IFileDownloadServices downloadServices, IStoreFilesServices storeFilesServices)
        {
            _hostingEnvironment = hostingEnvironment;
            _uploadServices = uploadServices;
            _downloadServices = downloadServices;
            _storeFilesServices = storeFilesServices;
        }


        #region 文件存储操作
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("GetPageList")]
        public async Task<HttpReponseViewModel<List<StoreFilesViewModel>>> GetPageList(QueryModel queryModel)
        {
            return await Task.Run(() =>
            {
                return _storeFilesServices.GetPageListService(queryModel);
            });

        }


        #endregion


        #region 文件上传操作
        #endregion


        #region 文件下载操作
        #endregion



    }
}