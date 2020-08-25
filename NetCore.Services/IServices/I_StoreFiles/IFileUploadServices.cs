using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.FileUpload;
using NetCore.DTO.RequestViewModel.FileUpload;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_StoreFiles
{
    public interface IFileUploadServices
    {
        HttpReponseViewModel<FileUploadResViewModel> CheckFileState(FileUploadReqViewModel fileUpload);

        Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload(FileUploadReqViewModel fileUpload);

        HttpReponseViewModel<FileUploadResViewModel> MergeFiles(FileUploadReqViewModel fileUpload);

        /// <summary>
        /// 返回最大的分片
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        HttpReponseViewModel<int> GetMaxChunk(FileUploadCheckChunkViewModel model);

        bool VaildMergeFile(FileUploadReqViewModel chunkFile);

    }
}
