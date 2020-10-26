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
        HttpReponseObjViewModel<List<int>> CheckFileState(HttpRequest request);

        Task<HttpReponseObjViewModel<bool>> ChunkUpload(IFormFile file, HttpRequest request);

        Task<HttpReponseObjViewModel<StoreFilesViewModel>> MergeFiles(FileUploadReqViewModel fileUpload);

        

        bool VaildMergeFile(FileUploadReqViewModel chunkFile);

    }
}
