using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
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
    public class StoreFilesServices:BaseServices<StoreFiles,StoreFilesViewModel>, IStoreFilesServices
    {
        private readonly IBaseDomain<StoreFiles> _baseDomain;
      
        public StoreFilesServices(IBaseDomain<StoreFiles> baseDomain) :base(baseDomain)
        {
            _baseDomain = baseDomain;
        }
       
    }
}
