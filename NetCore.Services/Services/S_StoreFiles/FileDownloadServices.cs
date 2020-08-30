using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.ReponseViewModel.FileDownload;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices;
using NetCore.Services.IServices.I_StoreFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NetCore.Services.Services.S_StoreFiles
{
    public class FileDownloadServices : IFileDownloadServices
    {
        private readonly IBaseDomain<StoreFiles> _baseDomain;
        public FileDownloadServices(IBaseDomain<StoreFiles> baseDomain)
        {
            _baseDomain = baseDomain;
        }
        public FileDownloadResViewModel GetFileInfoFromRequest(HttpRequest request, long entityLength)
        {
            var fileInfo = new FileDownloadResViewModel
            {
                From = 0,
                To = entityLength - 1,
                IsPartial = false,
                Length = entityLength
            };

            var requestHeaders = request.GetTypedHeaders();

            if (requestHeaders.Range != null && requestHeaders.Range.Ranges.Count > 0)
            {
                var range = requestHeaders.Range.Ranges.FirstOrDefault();
                if (range.From.HasValue && range.From < 0 || range.To.HasValue && range.To > entityLength - 1)
                {
                    return null;
                }

                var start = range.From;
                var end = range.To;

                if (start.HasValue)
                {
                    if (start.Value >= entityLength)
                    {
                        return null;
                    }
                    if (!end.HasValue || end.Value >= entityLength)
                    {
                        end = entityLength - 1;
                    }
                }
                else
                {
                    if (end.Value == 0)
                    {
                        return null;
                    }

                    var bytes = Math.Min(end.Value, entityLength);
                    start = entityLength - bytes;
                    end = start + bytes - 1;
                }

                fileInfo.IsPartial = true;
                fileInfo.Length = end.Value - start.Value + 1;
            }
            return fileInfo;
        }

        public void SetResponseHeaders(HttpResponse response, FileDownloadResViewModel fileInfo, string contentType, long fileLength, string fileName)
        {
            response.Headers[HeaderNames.AcceptRanges] = "bytes";
            response.StatusCode = fileInfo.IsPartial ? StatusCodes.Status206PartialContent
                                      : StatusCodes.Status200OK;

            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            response.Headers[HeaderNames.ContentType] = contentType;
            response.Headers[HeaderNames.ContentLength] = fileInfo.Length.ToString();
            if (fileInfo.IsPartial)
            {
                response.Headers[HeaderNames.ContentRange] = new ContentRangeHeaderValue(fileInfo.From, fileInfo.To, fileLength).ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public List<FileRangeViewModel> GetFileRangeData(long totalSize)
        {
            //分片大小
            int chunkSize = 1024 * 1024; //1M
            List<FileRangeViewModel> list = new List<FileRangeViewModel>();
            long currentSize = 0;
            int chunkIndex = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalSize / chunkSize)));
            for (int i = 0; i <= chunkIndex; i++)
            {
                if (currentSize > totalSize)
                {
                    break;
                }
                int startLength = 0;
                int endLength = 0;
                long tempSize = 0;
                FileRangeViewModel fileRangeView = new FileRangeViewModel();

                fileRangeView.SliceNumber = i + 1;
                if (i == 0)
                {
                    startLength = 0;
                    if (chunkSize>=totalSize)
                    {
                        endLength =Convert.ToInt32(totalSize)-1;
                        tempSize = totalSize-1;
                    }
                    else {
                        endLength = chunkSize;
                        tempSize = endLength;
                    }
                   
                }
                else
                {
                    startLength = (i * chunkSize);
                    endLength = (i + 1) * chunkSize;
                    if (endLength >= totalSize)
                    {
                        tempSize = totalSize - 1;
                    }
                    else
                    {
                        tempSize = endLength;
                    }
                }
                fileRangeView.ChunkSize = Convert.ToInt32(tempSize - startLength);
                fileRangeView.Range = "bytes=" + startLength + "-" + tempSize;
                currentSize = endLength;
                list.Add(fileRangeView);
            }

            return list;
        }


        public async Task<HttpReponseViewModel<string>> GetDownloadSmallFiles(Guid id, HttpContext httpContext)
        {
            HttpReponseViewModel<string> res = new HttpReponseViewModel<string>();
            var t = await _baseDomain.GetEntity(id);
            if (t != null)
            {
                var filePath = t.RelationFilePath;
                var response = httpContext.Response;
                var fileName = Path.GetFileName(filePath);//测试文档.xlsx
                int bufferSize = 10240 * 80;//这就是ASP.NET Core循环读取下载文件的缓存大小，这里我们设置为了1024字节，也就是说ASP.NET Core每次会从下载文件中读取1024字节的内容到服务器内存中，然后发送到客户端浏览器，这样避免了一次将整个下载文件都加载到服务器内存中，导致服务器崩溃
                response.ContentType = MimeMappingUtil.GetMimeMapping(filePath);//由于我们下载的是一个Excel文件，所以设置ContentType为application/vnd.ms-excel
                var contentDisposition = "attachment;" + "filename=" + HttpUtility.UrlEncode(fileName);//在Response的Header中设置下载文件的文件名，这样客户端浏览器才能正确显示下载的文件名，注意这里要用HttpUtility.UrlEncode编码文件名，否则有些浏览器可能会显示乱码文件名
                response.Headers.Add("Content-Disposition", new string[] { contentDisposition });
                //使用FileStream开始循环读取要下载文件的内容
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (response.Body)//调用Response.Body.Dispose()并不会关闭客户端浏览器到ASP.NET Core服务器的连接，之后还可以继续往Response.Body中写入数据
                    {
                        long contentLength = fs.Length;//获取下载文件的大小
                        response.ContentLength = contentLength;//在Response的Header中设置下载文件的大小，这样客户端浏览器才能正确显示下载的进度

                        byte[] buffer;
                        long hasRead = 0;//变量hasRead用于记录已经发送了多少字节的数据到客户端浏览器

                        //如果hasRead小于contentLength，说明下载文件还没读取完毕，继续循环读取下载文件的内容，并发送到客户端浏览器
                        while (hasRead < contentLength)
                        {
                            //HttpContext.RequestAborted.IsCancellationRequested可用于检测客户端浏览器和ASP.NET Core服务器之间的连接状态，如果HttpContext.RequestAborted.IsCancellationRequested返回true，说明客户端浏览器中断了连接
                            if (httpContext.RequestAborted.IsCancellationRequested)
                            {
                                //如果客户端浏览器中断了到ASP.NET Core服务器的连接，这里应该立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                                break;
                            }

                            buffer = new byte[bufferSize];

                            int currentRead = fs.Read(buffer, 0, bufferSize);//从下载文件中读取bufferSize(1024字节)大小的内容到服务器内存中

                            response.Body.Write(buffer, 0, currentRead);//发送读取的内容数据到客户端浏览器
                            response.Body.Flush();//注意每次Write后，要及时调用Flush方法，及时释放服务器内存空间

                            hasRead += currentRead;//更新已经发送到客户端浏览器的字节数
                        }
                    }
                }
            }
            else
            {
                res.Data = "该文件不存在";
            }
            res.Code = 20000;
            return res;
        }


        public async Task<HttpReponseViewModel<GenerateMD5ToBurstResViewModel>> GetMD5ToBurstData(Guid id)
        {
            HttpReponseViewModel<GenerateMD5ToBurstResViewModel> res = new HttpReponseViewModel<GenerateMD5ToBurstResViewModel>();
            var t = await _baseDomain.GetEntity(id);
            if (t != null)
            {
                var filePath = t.RelationFilePath;
                var fileName = t.FileName+"."+t.FileExt;
                var len = FileUtil.GetLength(filePath);
                res.Data = new GenerateMD5ToBurstResViewModel()
                {
                    Identifier = FileUploadUtil.GetMD5HashFromFile(filePath),
                    TotalSize = len,
                    FilePathUrl = filePath,
                    FileName =fileName,
                    FileRanges = GetFileRangeData(len)
                };
            }
            else
            {
                res.Data = null;
                res.Message = "该文件不存在";
            }
            res.Code = 20000;
            res.ResultSign = Core.Enum.ResultSign.Info;
            return res;
        }


        public async Task GetDownloadBigFiles(string path, HttpContext httpContext)
        {
            httpContext.Response.Headers["Etag"] = "ffffff";
            httpContext.Request.Headers["Last-Modified"] = new DateTime(2018).ToString("r");
            httpContext.Response.Headers["Accept-Ranges"] = "bytes";
            httpContext.Response.ContentType = MimeMappingUtil.GetMimeMapping(path);
            // 分段读取内容
            DownloadUtil download = new DownloadUtil(httpContext);
            await download.WriteFile(path);
        }
    }
}
