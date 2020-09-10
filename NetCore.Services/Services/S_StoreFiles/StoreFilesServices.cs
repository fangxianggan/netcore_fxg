using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.EntityFrameworkCore.Models;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_StoreFiles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_StoreFiles
{
    public class StoreFilesServices : BaseServices<StoreFiles, StoreFilesViewModel>, IStoreFilesServices
    {
        private readonly IBaseDomain<StoreFiles> _baseDomain;

        public StoreFilesServices(IBaseDomain<StoreFiles> baseDomain) : base(baseDomain)
        {
            _baseDomain = baseDomain;
        }

        public async Task<HttpReponseViewModel> DeleteStoreFiles(Guid id)
        {
            HttpReponseViewModel httpReponse = null;
            var ent = await _baseDomain.GetEntity(id);
            var urlPath =  ent.RelationFilePath;
            //删除文件
            FileUtil.DeleteFiles(urlPath);
            var flag = FileUtil.IsExistFile(urlPath);
            if (!flag)
            {
                var f = await _baseDomain.DeleteDomain(id);
                httpReponse = new HttpReponseViewModel(f);
            }
            else {
                httpReponse = new HttpReponseViewModel(false,"","文件未删除");
            }
            return httpReponse;

        }
    }
}
