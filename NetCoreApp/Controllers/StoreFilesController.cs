using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.FileUpload;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.DTO.RequestViewModel.FileUpload;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_StoreFiles;
using NetCoreApp.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Controllers
{

    /// <summary>
    /// 文件存储
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]

    public class StoreFilesController : ControllerBase
    {
        private static IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFileUploadServices _uploadServices;
        private readonly IFileDownloadServices _downloadServices;
        private readonly IStoreFilesServices _storeFilesServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="uploadServices"></param>
        /// <param name="downloadServices"></param>
        /// <param name="storeFilesServices"></param>
        public StoreFilesController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, IFileUploadServices uploadServices, IFileDownloadServices downloadServices, IStoreFilesServices storeFilesServices)
        {
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _uploadServices = uploadServices;
            _downloadServices = downloadServices;
            _storeFilesServices = storeFilesServices;
        }


        #region 文件存储操作
        /// <summary>
        /// 文件列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        /// 
        [TypeFilter(typeof(CustomerExceptionFilter))]
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

        /// <summary>
        /// 检查 断点续传 和 秒传
        /// </summary>
        /// <param name="chunkNumber"></param>
        /// <param name="chunkSize"></param>
        /// <param name="currentChunkSize"></param>
        /// <param name="totalSize"></param>
        /// <param name="identifier"></param>
        /// <param name="filename"></param>
        /// <param name="relativePath"></param>
        /// <param name="totalChunks"></param>
        /// <returns></returns>
        [HttpGet, Route("ChunkUpload")]
        public async Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload(int chunkNumber, int chunkSize, int currentChunkSize, int totalSize, string identifier, string filename, string relativePath, int totalChunks)
        {
            FileUploadReqViewModel fileUpload = new FileUploadReqViewModel();
            fileUpload.Identifier = identifier;
            fileUpload.FileName = filename;
            fileUpload.ChunkNumber = chunkNumber;
            fileUpload.ChunkNumber = chunkSize;
            fileUpload.CurrentChunkSize = currentChunkSize;
            fileUpload.RelativePath = relativePath;
            fileUpload.TotalChunks = totalChunks;
            fileUpload.TotalSize = totalSize;
            return await Task.Run(() =>
            {
                return _uploadServices.CheckFileState(fileUpload);
            });
        }

        /// <summary>
        /// 文件分块上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, Route("ChunkUpload")]
        public async Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload(IFormFile file)
        {
            HttpReponseViewModel<FileUploadResViewModel> res = new HttpReponseViewModel<FileUploadResViewModel>();
            return await Task.Run(() =>
            {
                return _uploadServices.ChunkUpload(file, _contextAccessor.HttpContext.Request);
            });

        }



        /// <summary>
        ///  合并文件
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        [HttpPost, Route("MergeFiles")]
        public async Task<HttpReponseViewModel<FileUploadResViewModel>> MergeFiles(FileUploadReqViewModel fileUpload)
        {
            return await Task.Run(() =>
            {
                return _uploadServices.MergeFiles(fileUpload);
            });

        }


        #endregion


        #region 文件下载操作
        #endregion



    }
}