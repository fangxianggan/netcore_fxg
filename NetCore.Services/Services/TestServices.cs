using NetCore.Core.Extensions;
using NetCore.Domain.Interface;
using NetCore.DTO.TestModel;
using NetCore.EntityFrameworkCore.Models;
using NetCore.IServices;
using NetCore.Repository.AspectCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Services
{
    public class TestServices : ITestServices
    {
        private readonly IBaseDomain<Test> _userDomain;
      
        public TestServices(IBaseDomain<Test> userDomain)
        {
            _userDomain = userDomain;
        }
       
     //  [Transactional]
        public async Task<bool> AddService(TestViewModel entity)
        {

            var t1 = entity.MapTo<Test>();
            var dd = await _userDomain.AddDomain(t1);


            var t2 = entity.MapTo<Test>();
            var d3 = await _userDomain.AddDomain(t1);
            return true;
        }

       // [Transactional]
        public async Task<bool> AddListService(List<TestViewModel> entity)
        {
            var t2 = entity.MapTo<List<Test>>();
            var d3 = await _userDomain.AddListDomain(t2);

            var t23 = entity.MapTo<List<Test>>();
            var d33 = await _userDomain.AddListDomain(t23);
            return true;
        }

    }
}
