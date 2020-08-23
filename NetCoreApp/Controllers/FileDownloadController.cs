using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Util;
using NetCore.DTO.ReponseViewModel;
using NetCore.Services.IServices;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    public class FileDownloadController : ControllerBase
    {
        private const int BufferSize = 80 * 1024;
        private readonly IFileDownloadServices _downloadServices;

        private const string MimeType = "application/octet-stream";

        private const string AppSettingDirPath = "DownloadDir";

        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 
        /// </summary>
        public FileDownloadController(IHttpContextAccessor contextAccessor, IFileDownloadServices downloadServices)
        {
            _downloadServices = downloadServices;
            _contextAccessor = contextAccessor;
        }
        /// <summary>
        /// 普通下载文件方式 分片加载内存 不适合大文件下载
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("DownloadBigFile")]
        public IActionResult DownloadBigFile()
        {
            var filePath = @"D:\Files\BI_V2.0.zip";//要下载的文件地址，这个文件会被分成片段，通过循环逐步读取到ASP.NET Core中，然后发送给客户端浏览器
            var fileName = Path.GetFileName(filePath);//测试文档.xlsx

            int bufferSize = 10240 * 80;//这就是ASP.NET Core循环读取下载文件的缓存大小，这里我们设置为了1024字节，也就是说ASP.NET Core每次会从下载文件中读取1024字节的内容到服务器内存中，然后发送到客户端浏览器，这样避免了一次将整个下载文件都加载到服务器内存中，导致服务器崩溃

            Response.ContentType = MimeMappingUtil.GetMimeMapping(filePath);//由于我们下载的是一个Excel文件，所以设置ContentType为application/vnd.ms-excel

            var contentDisposition = "attachment;" + "filename=" + HttpUtility.UrlEncode(fileName);//在Response的Header中设置下载文件的文件名，这样客户端浏览器才能正确显示下载的文件名，注意这里要用HttpUtility.UrlEncode编码文件名，否则有些浏览器可能会显示乱码文件名
            Response.Headers.Add("Content-Disposition", new string[] { contentDisposition });

            //使用FileStream开始循环读取要下载文件的内容
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (Response.Body)//调用Response.Body.Dispose()并不会关闭客户端浏览器到ASP.NET Core服务器的连接，之后还可以继续往Response.Body中写入数据
                {
                    long contentLength = fs.Length;//获取下载文件的大小
                    Response.ContentLength = contentLength;//在Response的Header中设置下载文件的大小，这样客户端浏览器才能正确显示下载的进度

                    byte[] buffer;
                    long hasRead = 0;//变量hasRead用于记录已经发送了多少字节的数据到客户端浏览器

                    //如果hasRead小于contentLength，说明下载文件还没读取完毕，继续循环读取下载文件的内容，并发送到客户端浏览器
                    while (hasRead < contentLength)
                    {
                        //HttpContext.RequestAborted.IsCancellationRequested可用于检测客户端浏览器和ASP.NET Core服务器之间的连接状态，如果HttpContext.RequestAborted.IsCancellationRequested返回true，说明客户端浏览器中断了连接
                        if (HttpContext.RequestAborted.IsCancellationRequested)
                        {
                            //如果客户端浏览器中断了到ASP.NET Core服务器的连接，这里应该立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                            break;
                        }

                        buffer = new byte[bufferSize];

                        int currentRead = fs.Read(buffer, 0, bufferSize);//从下载文件中读取bufferSize(1024字节)大小的内容到服务器内存中

                        Response.Body.Write(buffer, 0, currentRead);//发送读取的内容数据到客户端浏览器
                        Response.Body.Flush();//注意每次Write后，要及时调用Flush方法，及时释放服务器内存空间

                        hasRead += currentRead;//更新已经发送到客户端浏览器的字节数
                    }
                }
            }

            return new EmptyResult();
        }




        /// <summary>
        /// 下载文件 分片下载 适合大文件下载
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("lldownload")]
        public IActionResult GetFile()
        {
            string fileName = @"D:\Files\BI_V2.0.zip";
            if (!FileUtil.IsExistFile(fileName))
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            //获取下载文件长度
            var fileLength = FileUtil.GetLength(fileName);
            //初始化下载文件信息
            var fileInfo = _downloadServices.GetFileInfoFromRequest(_contextAccessor.HttpContext.Request, fileLength);
            //获取剩余部分文件流
            var stream = new PartialContentFileStream(FileUtil.GetStreamFrom(fileName), fileInfo.From, fileInfo.To);
            var contentType = MimeMappingUtil.GetMimeMapping(fileName);
            //设置响应 请求头
            _downloadServices.SetResponseHeaders(_contextAccessor.HttpContext.Response, fileInfo, contentType, fileLength, fileName);
            return new FileStreamResult(stream, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(contentType));
        }



        #region 分段下载 支持断点续载
        /// <summary>
        /// 拆分分片信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet, Route("GetMD5ToBurstData")]
        public async Task<HttpReponseViewModel<GenerateMD5ToBurstResViewModel>> GetMD5ToBurstData(string filePath)
        {
            filePath = @"D:\Files\BI_V2.0.zip";
            //filePath = @"D:\Files\Files.zip";
            HttpReponseViewModel<GenerateMD5ToBurstResViewModel> res = new HttpReponseViewModel<GenerateMD5ToBurstResViewModel>();
            return await Task.Run(() =>
            {
                long len = FileUtil.GetLength(filePath);
                res.Code = 20000;
                res.Data = new GenerateMD5ToBurstResViewModel()
                {
                    Identifier = FileUploadUtil.GetMD5HashFromFile(filePath),
                    TotalSize = len,
                    FileName = filePath,
                    FileRanges = _downloadServices.GetFileRangeData(len)
                };
                return res;
            });
        }

        /// <summary>
        /// 分片下载
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("DownLoad2")]
        public async Task DownLoad2()
        {

            string path = @"D:\Files\BI_V2.0.zip";
            int length = path.LastIndexOf(".") - path.LastIndexOf("/") - 1;
            _contextAccessor.HttpContext.Response.Headers["Etag"] = "ffffff";
            _contextAccessor.HttpContext.Request.Headers["Last-Modified"] = new DateTime(2018).ToString("r");
            _contextAccessor.HttpContext.Response.Headers["Accept-Ranges"] = "bytes";
            _contextAccessor.HttpContext.Response.ContentType = MimeMappingUtil.GetMimeMapping(path);
            // 分段读取内容

            DownloadUtil download = new DownloadUtil(_contextAccessor.HttpContext);
            await download.WriteFile(path);




            //return await Task.Run(() =>
            //{
            //    string path = @"D:\Files\BI_V2.0.zip";

            //    int length = path.LastIndexOf(".") - path.LastIndexOf("/") - 1;
            //    _contextAccessor.HttpContext.Response.Headers["Etag"] = "ffffff";
            //    _contextAccessor.HttpContext.Request.Headers["Last-Modified"] = new DateTime(2018).ToString("r");
            //    _contextAccessor.HttpContext.Response.Headers["Accept-Ranges"] = "bytes";
            //    _contextAccessor.HttpContext.Response.ContentType = MimeMappingUtil.GetMimeMapping(path);
            //    DownloadRange download = new DownloadRange(_contextAccessor.HttpContext);
            //    download.WriteFile(path);
            //    return new EmptyResult();
            //});


        }
        #endregion


    }
}