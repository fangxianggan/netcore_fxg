using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.RequestViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.IServices
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
