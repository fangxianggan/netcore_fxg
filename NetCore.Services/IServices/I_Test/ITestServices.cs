using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.TestModel;
using NetCore.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.IServices.I_Test
{
    public  interface ITestServices:IBaseServices<TestViewModel>
    {
        Task<bool> AddListService1(List<TestViewModel> entity);

        Task<HttpReponseObjViewModel<TestViewModel>> AddOrEditService1(TestViewModel entity);

    }
}
