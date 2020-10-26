using Microsoft.AspNetCore.Http;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.ReponseViewModel.FileDownload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_StoreFiles
{
    public interface IFileDownloadServices
    {
        FileDownloadResViewModel GetFileInfoFromRequest(HttpRequest request, long entityLength);

        void SetResponseHeaders(HttpResponse response, FileDownloadResViewModel fileInfo, string contentType,
                                      long fileLength, string fileName);

        List<FileRangeViewModel> GetFileRangeData(long totalSize);

        Task<HttpReponseObjViewModel<string>> GetDownloadSmallFiles(Guid id,HttpContext httpContext);

        Task<HttpReponseObjViewModel<GenerateMD5ToBurstResViewModel>> GetMD5ToBurstData(Guid id);

        Task GetDownloadBigFiles(string path, HttpContext httpContext);

    }
}
