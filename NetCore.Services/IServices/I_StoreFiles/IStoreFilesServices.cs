using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_StoreFiles
{
   public interface IStoreFilesServices:IBaseServices<StoreFilesViewModel>
    {
        Task<HttpReponseViewModel> DeleteStoreFiles(Guid id);
    }
}
