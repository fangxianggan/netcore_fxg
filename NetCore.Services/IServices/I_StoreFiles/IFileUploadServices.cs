using Microsoft.AspNetCore.Http;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.FileUpload;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.DTO.RequestViewModel.FileUpload;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_StoreFiles
{
    public interface IFileUploadServices
    {
        HttpReponseViewModel<List<int>> CheckFileState(HttpRequest request);

        Task<HttpReponseViewModel<bool>> ChunkUpload(IFormFile file, HttpRequest request);

        Task<HttpReponseViewModel<StoreFilesViewModel>> MergeFiles(FileUploadReqViewModel fileUpload);

        

        bool VaildMergeFile(FileUploadReqViewModel chunkFile);

    }
}
