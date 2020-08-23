using NetCore.EntityFrameworkCore.EntityModels;
using NetCore.EntityFrameworkCore.Interface;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.EntityFrameworkCore.Models
{
    /// <summary>
    /// 任务的类
    /// </summary>
    /// 
    [Table("TaskJob")]
    public partial class TaskJob : BaseEntity<Guid>, ISoftDelete
    {


        /// <summary>
        /// 任务组
        /// </summary>
        /// 
        [DisplayName("任务组"), MaxLength(64)]
        public string TaskGroup { set; get; }

        /// <summary>
        /// 任务名称
        /// </summary>
        /// 
        [DisplayName("任务名称"), MaxLength(64)]
        public string TaskName { set; get; }

        /// <summary>
        /// 任务的描述
        /// </summary>
        /// 
        [DisplayName("任务描述"), MaxLength(256)]
        public string Description { set; get; }

        /// <summary>
        /// 接口地址
        /// </summary>
        /// 
        [DisplayName("接口地址"), MaxLength(256)]
        public string ApiUrl { set; get; }

        /// <summary>
        /// 请求类型
        /// </summary>
        /// 
        [DisplayName("请求类型"), MaxLength(8)]
        public string RequestType { set; get; }

        /// <summary>
        /// 请求头
        /// </summary>
        /// 
        [DisplayName("请求头"), MaxLength(1024)]
        public string RequestHead { set; get; }

        /// <summary>
        /// 请求的参数 以json形式传入
        /// </summary>
        /// 
        [DisplayName("请求参数")]
        public string RequestParams { set; get; }

        /// <summary>
        /// 异常信息说明
        /// </summary>
        /// 
        [DisplayName("异常信息")]
        public string ExceptionMsg { set; get; }

        /// <summary>
        /// Cron 表达式
        /// </summary>
        /// 
        [DisplayName("Cron表达式"), MaxLength(512)]
        public string CronExpression { set; get; }

        /// <summary>
        ///Cron 表达式描述
        /// </summary>
        /// 
        [DisplayName("Cron描述"), MaxLength(512)]
        public string CronExpressionDescription { set; get; }
        /// <summary>
        /// 开始执行时间
        /// </summary>
        /// 
        [DisplayName("开始执行时间")]
        public DateTime? StartRunTime { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// 
        [DisplayName("结束时间")]
        public DateTime? EndRunTime { set; get; }

        /// <summary>
        /// 运行次数
        /// </summary>
        /// 
        [DisplayName("运行次数")]
        public int RunCount { set; get; }

        /// <summary>
        /// 任务状态
        /// </summary>
        /// 
        [DisplayName("任务状态"), MaxLength(8)]
        public string TaskState { set; get; }

        /// <summary>
        ///是否 删除
        /// </summary>
        public bool IsDelete { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
