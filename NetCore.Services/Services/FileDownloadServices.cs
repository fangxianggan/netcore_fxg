using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using NetCore.DTO.ReponseViewModel;
using NetCore.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore.Services.Services
{
    public class FileDownloadServices : IFileDownloadServices
    {
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
            int chunkSize = 1024 * 1024 ; //1M
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
                    endLength = chunkSize;
                    tempSize = endLength;
                }
                else
                {
                    startLength = (i * chunkSize);
                    endLength = (i + 1) * chunkSize;
                    if (endLength >= totalSize)
                    {
                        tempSize = totalSize-1;
                    }
                    else {
                        tempSize = endLength;
                    }
                }
                fileRangeView.ChunkSize =Convert.ToInt32(tempSize- startLength);
                fileRangeView.Range = "bytes=" + startLength + "-" + tempSize;
                currentSize = endLength;
                list.Add(fileRangeView);
            }

            return list;
        }
    }
}
