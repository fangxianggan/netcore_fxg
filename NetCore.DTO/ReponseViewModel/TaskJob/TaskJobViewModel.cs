using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.TaskJob
{
   public class TaskJobViewModel
    {
        public Guid ID { set; get; }

        /// <summary>
        /// 任务组
        /// </summary>
        /// 
  
        public string TaskGroup { set; get; }

        /// <summary>
        /// 任务名称
        /// </summary>
        /// 
      
        public string TaskName { set; get; }

        /// <summary>
        /// 任务的描述
        /// </summary>
        /// 
      
        public string Description { set; get; }

        /// <summary>
        /// 接口地址
        /// </summary>
        /// 
     
        public string ApiUrl { set; get; }

        /// <summary>
        /// 请求类型
        /// </summary>
        /// 
       
        public string RequestType { set; get; }

        /// <summary>
        /// 请求头
        /// </summary>
        /// 
      
        public string RequestHead { set; get; }

        /// <summary>
        /// 请求的参数 以json形式传入
        /// </summary>
        /// 
      
        public string RequestParams { set; get; }

        /// <summary>
        /// 异常信息说明
        /// </summary>
        /// 
       
        public string ExceptionMsg { set; get; }

        /// <summary>
        /// Cron 表达式
        /// </summary>
        /// 
      
        public string CronExpression { set; get; }

        /// <summary>
        ///Cron 表达式描述
        /// </summary>
        /// 
      
        public string CronExpressionDescription { set; get; }
        /// <summary>
        /// 开始执行时间
        /// </summary>
        /// 
     
        public DateTime? StartRunTime { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// 
      
        public DateTime? EndRunTime { set; get; }

        /// <summary>
        /// 运行次数
        /// </summary>
        /// 
     
        public int RunCount { set; get; }

        /// <summary>
        /// 任务状态
        /// </summary>
        /// 
      
        public string TaskState { set; get; }
    }
}
