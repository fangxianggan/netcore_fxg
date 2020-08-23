using Microsoft.AspNetCore.Http;
using NetCore.DTO.ReponseViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.IServices
{
    public interface IFileDownloadServices
    {
        FileDownloadResViewModel GetFileInfoFromRequest(HttpRequest request, long entityLength);

        void SetResponseHeaders(HttpResponse response, FileDownloadResViewModel fileInfo, string contentType,
                                      long fileLength, string fileName);

        List<FileRangeViewModel> GetFileRangeData(long totalSize);
    }
}
