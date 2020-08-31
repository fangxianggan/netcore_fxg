using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.TestModel;
using NetCore.IServices.I_Test;
using NetCoreApp.Filters;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestServices _testService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testService"></param>
        public TestController(ITestServices testService)
        {
            _testService = testService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("/fff")]
        [HttpGet]
        public async Task<HttpReponseViewModel<TestViewModel>> GetT()
        {
            TestViewModel model = new TestViewModel();
            model.ID = Guid.NewGuid();
            model.Name = "ffff";
            return await _testService.AddOrEditService(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [TypeFilter(typeof(CustomerExceptionFilter))]
        [Route("/fff2")]
        [HttpGet]
        public async Task<bool> GetTT()
        {
            
            var list = new List<TestViewModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TestViewModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "ffff" + i
                });
            }

            return await _testService.AddListService(list);
        }
    }





}