using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.TaskJob
{
   public class TaskJobLogViewModel
    {

        public Guid ID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("任务Id")]
        public Guid TaskJobId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("任务名称"), MaxLength(64)]
        public string JobName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("执行时间")]
        public DateTime ExecutionTime { set; get; }
        /// <summary>
        /// /
        /// </summary>
        [DisplayName("执行时间长度")]
        public double ExecutionDuration { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("运行日志"), MaxLength]
        public string RunLog { set; get; }

      
    }
}
