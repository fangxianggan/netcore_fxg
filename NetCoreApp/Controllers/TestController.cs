using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.RabbitMQ;
using NetCore.Core.Util;
using NetCore.DTO.TestModel;
using NetCore.IServices.I_Test;
using NetCore.Services.Interface;
using NetCore.Services.IServices.I_RabbitMq;
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
        private readonly ILogger logger;
        private readonly IProducerMqServices _producerMqServices;
       // private readonly IRabbitMQClientTest _rabbitMQClientTest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="producerMqServices"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="testService"></param>
        /// <param name="contextAccessor"></param>
        public TestController(IProducerMqServices producerMqServices,ILoggerFactory loggerFactory,ITestServices testService, IHttpContextAccessor contextAccessor)
        {
           // _rabbitMQClientTest = rabbitMQClientTest;
            _testService = testService;
            _contextAccessor = contextAccessor;
            logger = loggerFactory.CreateLogger<RabbitLogger>();
            _producerMqServices = producerMqServices;
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

        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>

        [Route("/fff5")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult fff5(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                //发送消息,这里也可以检验日志级别的过滤
                logger.LogCritical($"Log from Critical:{message}");
                logger.LogDebug($"Log from Debug:{message}");
                logger.LogError($"Log from Error:{message}");
                logger.LogInformation($"Log from Information:{message}");
                logger.LogTrace($"Log from Trace:{message}");
                logger.LogWarning($"Log from Warning:{message}");
            }
            return Content(message);
        }

        /// <summary>
        /// 消息队列测试  new
        /// </summary>
        /// <returns></returns>
        [Route("/fff6")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult fff6()
        {
            _producerMqServices.ProducerMesTest();
            return Content("ok");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("/fff7")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult fff7()
        {
          //  _rabbitMQClientTest.test();
            return Content("ok");
        }

    }





}