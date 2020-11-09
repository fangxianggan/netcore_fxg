//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using NetCore.Core.RabbitMQ;
//using NetCore.Core.Util;
//using NetCore.Services.IServices.I_RabbitMq;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace NetCore.Services.Services.S_RabbitMq
//{
//    public class ChapterLister : RabbitListener, IChapterListerTest
//    {
//        public ChapterLister(IOptions<RabbitMQLoggerOptions> options) : base(options)
//        {
//            //base.RouteKey = "done.task";
//            base.QueueName = options.Value.Queue;


//        }

//        public override bool Process(string message)
//        {
//            var taskMessage = message;
//            if (taskMessage == null)
//            {
//                // 返回false 的时候回直接驳回此消息,表示处理不了
//                return false;
//            }
//            try
//            {
//                var mes = DateTime.Now + "_" + message;
//                LogUtil.Info(mes);

//                Test();

//                return true;

//            }
//            catch (Exception ex)
//            {
//                LogUtil.Error($"Process fail,error:{ex.Message},stackTrace:{ex.StackTrace},message:{message}");
//                return false;
//            }

//        }

//        public string Test()
//        {
//            return "1111";
//        }
//    }
//}
