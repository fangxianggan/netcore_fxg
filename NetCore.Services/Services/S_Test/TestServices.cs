using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Extensions;
using NetCore.Domain.Interface;
using NetCore.DTO.TestModel;
using NetCore.EntityFrameworkCore.Models;
using NetCore.EntityModel.QueryModels;
using NetCore.IServices.I_Test;
using NetCore.Services.Interface;
using NetCore.Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services.S_Test
{
    public class TestServices :BaseServices<Test,TestViewModel>, ITestServices
    {
        private readonly IBaseDomain<Test> _userDomain;
        public TestServices(IBaseDomain<Test> userDomain):base(userDomain)
        {
            _userDomain = userDomain;
        }

        // [Transactional]
        public async Task<bool> AddListService1(List<TestViewModel> entity)
        {
            var t2 = entity.MapTo<List<Test>>();
            var d3 = await _userDomain.AddListDomain(t2);

            var t23 = entity.MapTo<List<Test>>();
            var d33 = await _userDomain.AddListDomain(t23);
            return true;
        }


       



        //  [Transactional]

        public async Task<HttpReponseViewModel<TestViewModel>> AddOrEditService1(TestViewModel entity)
        {

            HttpReponseViewModel<TestViewModel> res = new HttpReponseViewModel<TestViewModel>();
            var t1 = entity.MapTo<Test>();
            var flag = await _userDomain.AddDomain(t1);

            var t2 = entity.MapTo<Test>();
            var flag1 = await _userDomain.AddDomain(t1);

            return res;
        }

      
    }
}
