using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Util;
using NetCore.DTO.TestModel;
using NetCore.IServices.I_Test;
using NetCore.Services.Interface;
using NetCoreApp.Filters;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 

    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize] // 添加授权特性
    public class TestController : ControllerBase
    {
        private readonly ITestServices _testService;
        private readonly IHttpContextAccessor _contextAccessor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testService"></param>
        /// <param name="contextAccessor"></param>
        public TestController(ITestServices testService, IHttpContextAccessor contextAccessor)
        {
            _testService = testService;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("/fff")]
        [HttpGet]
        public async Task<HttpReponseObjViewModel<TestViewModel>> GetT()
        {
            TestViewModel model = new TestViewModel();
            model.ID = Guid.NewGuid();
            model.Name = "ffff";
            return await _testService.AddOrEditService1(model);
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
            for (int i = 0; i < 100; i++)
            {
                list.Add(new TestViewModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "ffff" + i
                });
            }

            await _testService.AddOrEditService1(list[0]);
            var list1 = new List<TestViewModel>();
            for (int i = 100; i < 200; i++)
            {
                list1.Add(new TestViewModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "ffff" + i
                });
            }
            await _testService.AddOrEditService1(list1[1]);
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CustomerExceptionFilter))]
        [Route("/fff3")]
        [HttpGet]
        public string UserInfo()
        {
            var bb = _contextAccessor.HttpContext.User.Claims.ToList();
            var cc = bb;
            return JsonUtil.JsonSerialize(cc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [CacheResourceFilterAttribute]
        //[TypeFilter(typeof(CustomerExceptionFilter))]
        [Route("/fff4")]
        [HttpGet]
        [ResultFilter("X-Value", "Y-Value")]
        public IActionResult fff4()
        {
            Thread.Sleep(1000);
            var bb = 0;
            var cc = bb / 100;
            return Content("ok");

           
        }
    }





}