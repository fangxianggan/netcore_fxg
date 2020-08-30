using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Util;
using NetCore.DTO.ReponseViewModel.FileDownload;
using NetCore.DTO.ReponseViewModel.FileUpload;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.DTO.RequestViewModel.FileUpload;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_StoreFiles;
using NetCoreApp.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

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


        #region 一次性上传
        /// <summary>
        /// Form表单之单文件上传
        /// </summary>
        /// <param name="formFile">form表单文件流信息</param>
        /// <returns></returns>
        /// 
        [HttpPost, Route("FormSingleFileUpload")]
        public JsonResult FormSingleFileUpload(IFormFile formFile)
        {
            var currentDate = DateTime.Now;
            var webRootPath = _hostingEnvironment.WebRootPath;//>>>相当于HttpContext.Current.Server.MapPath("") 
            try
            {
                var filePath = $"/UploadFile/{currentDate:yyyyMMdd}/";

                //创建每日存储文件夹
                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                if (formFile != null)
                {
                    //文件后缀
                    var fileExtension = Path.GetExtension(formFile.FileName);//获取文件格式，拓展名

                    //判断文件大小
                    var fileSize = formFile.Length;

                    if (fileSize > 1024 * 1024 * 10) //10M TODO:(1mb=1024X1024b)
                    {
                        return new JsonResult(new { isSuccess = false, resultMsg = "上传的文件不能大于10M" });
                    }

                    //保存的文件名称(以名称和保存时间命名)
                    var saveName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.')) + "_" + currentDate.ToString("HHmmss") + fileExtension;

                    //文件保存
                    using (var fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    //完整的文件路径
                    var completeFilePath = Path.Combine(filePath, saveName);

                    return new JsonResult(new { isSuccess = true, returnMsg = "上传成功", completeFilePath = completeFilePath });
                }
                else
                {
                    return new JsonResult(new { isSuccess = false, resultMsg = "上传失败，未检测上传的文件信息~" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { isSuccess = false, resultMsg = "文件保存失败，异常信息为：" + ex.Message });
            }
        }



        #endregion

        #region 文件上传操作

        /// <summary>
        /// 检查 断点续传 和 秒传
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("ChunkUpload")]
        public async Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload()
        {
            return await Task.Run(() =>
            {
                return _uploadServices.CheckFileState(_contextAccessor.HttpContext.Request);
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
        public async Task<HttpReponseViewModel<StoreFilesViewModel>> MergeFiles(FileUploadReqViewModel fileUpload)
        {
            return await Task.Run(() =>
            {
                return _uploadServices.MergeFiles(fileUpload);
            });

        }
        #endregion

        #region 文件下载操作

        /// <summary>
        /// 普通下载文件方式 分片加载内存 不适合大文件下载
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("DownloadSmallFiles")]
        public async Task<HttpReponseViewModel<string>> DownloadSmallFiles(string id)
        {
            Guid gid = new Guid(id);
            return await Task.Run(() =>
            {
                return _downloadServices.GetDownloadSmallFiles(gid, _contextAccessor.HttpContext);
            });
        }


        /// <summary>
        /// 拆分分片信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [TypeFilter(typeof(CustomerExceptionFilter))]
        [HttpGet, Route("GetMD5ToBurstData")]
        public async Task<HttpReponseViewModel<GenerateMD5ToBurstResViewModel>> GetMD5ToBurstData(Guid id)
        {
            return await Task.Run(() =>
            {
                return _downloadServices.GetMD5ToBurstData(id);
            });
        }

        /// <summary>
        /// 分片下载
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetDownloadBigFiles")]
        public async Task GetDownloadBigFiles(string path)
        {
            var httpContext = _contextAccessor.HttpContext;
            httpContext.Response.Headers["Etag"] = "ffffff";
            httpContext.Request.Headers["Last-Modified"] = new DateTime(2018).ToString("r");
            httpContext.Response.Headers["Accept-Ranges"] = "bytes";
            httpContext.Response.ContentType = MimeMappingUtil.GetMimeMapping(path);
            // 分段读取内容
            DownloadUtil download = new DownloadUtil(httpContext);
            await download.WriteFile(path);

        }

        #endregion



    }
}