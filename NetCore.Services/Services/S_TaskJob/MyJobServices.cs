using NetCore.Core.Extensions;
using NetCore.Core.Helper;
using NetCore.Core.Util;
using NetCore.DTO.Enum;
using NetCore.DTO.ReponseViewModel.TaskJob;
using NetCore.Services.IServices.I_TaskJob;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NetCore.Services.Services.S_TaskJob
{

    /// <summary>
    /// 
    /// </summary>
    //前一次任务未执行完成时不触发下次
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class MyJobServices : IJob, IMyJobServices
    {
      
        public async Task Execute(IJobExecutionContext context)
        {

            //var gId = new Guid(context.JobDetail.Key.Name.Split('|')[0]);
            // LogUtil.Debug("陈大猪。。。" + gId);
            //LogUtil.Debug("ManagerJob Executing ...");
            //  await _taskJobServices.ExcuteTaskJob(gId);

            var maxLogCount = 20;//最多保存日志数量
            var warnTime = 20;//接口请求超过多少秒记录警告日志         
                              //获取相关参数
            var requestUrl = context.JobDetail.JobDataMap.GetString(ConstantUtil.REQUESTURL);
            requestUrl = requestUrl?.IndexOf("http") == 0 ? requestUrl : "http://" + requestUrl;
            var requestParameters = context.JobDetail.JobDataMap.GetString(ConstantUtil.REQUESTPARAMETERS);
            var headersString = context.JobDetail.JobDataMap.GetString(ConstantUtil.HEADERS);
            var mailMessage = (MailMessageEnum)int.Parse(context.JobDetail.JobDataMap.GetString(ConstantUtil.MAILMESSAGE) ?? "0");
            var headers = headersString != null ? JsonUtil.JsonDeserializeObject<Dictionary<string, string>>(headersString?.Trim()) : null;
            var requestType = (RequestTypeEnum)int.Parse(context.JobDetail.JobDataMap.GetString(ConstantUtil.REQUESTTYPE));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart(); //  开始监视代码运行时间
            HttpResponseMessage response = new HttpResponseMessage();

            var loginfo = new LogInfoViewModel();
            loginfo.Url = requestUrl;
            loginfo.BeginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            loginfo.RequestType = requestType.ToString();
            loginfo.Parameters = requestParameters;
            loginfo.JobName = $"{context.JobDetail.Key.Group}.{context.JobDetail.Key.Name}";

            var logs = context.JobDetail.JobDataMap[ConstantUtil.LOGLIST] as List<string> ?? new List<string>();
            if (logs.Count >= maxLogCount)
                logs.RemoveRange(0, logs.Count - maxLogCount);
            //业务
            try
            {
                var http = HttpHelper.Instance;
                switch (requestType)
                {
                    case RequestTypeEnum.Get:
                        response = await http.GetAsync(requestUrl, headers);
                        break;
                    case RequestTypeEnum.Post:
                        response = await http.PostAsync(requestUrl, requestParameters, headers);
                        break;
                    case RequestTypeEnum.Put:
                        response = await http.PutAsync(requestUrl, requestParameters, headers);
                        break;
                    case RequestTypeEnum.Delete:
                        response = await http.DeleteAsync(requestUrl, headers);
                        break;
                }
                var result = HttpUtility.HtmlEncode(await response.Content.ReadAsStringAsync());

                stopwatch.Stop(); //  停止监视            
                double seconds = stopwatch.Elapsed.TotalSeconds;  //总秒数                                
                loginfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                loginfo.Seconds = seconds;
                loginfo.Result = $"<span class='result'>{result.MaxLeft(1000)}</span>";
                if (!response.IsSuccessStatusCode)
                {
                    loginfo.ErrorMsg = $"<span class='error'>{result.MaxLeft(3000)}</span>";
                    await ErrorAsync(loginfo.JobName, new Exception(result.MaxLeft(3000)), JsonUtil.JsonSerialize(loginfo), mailMessage);
                    context.JobDetail.JobDataMap[ConstantUtil.EXCEPTION] = JsonUtil.JsonSerialize(loginfo);
                }
                else
                {
                    try
                    {
                        //这里需要和请求方约定好返回结果约定为HttpResultViewModel模型
                        var httpResult = JsonUtil.JsonDeserializeObject<HttpResultViewModel>(HttpUtility.HtmlDecode(result));
                        if (!httpResult.IsSuccess)
                        {
                            loginfo.ErrorMsg = $"<span class='error'>{httpResult.ErrorMsg}</span>";
                            await ErrorAsync(loginfo.JobName, new Exception(httpResult.ErrorMsg), JsonUtil.JsonSerialize(loginfo), mailMessage);
                            context.JobDetail.JobDataMap[ConstantUtil.EXCEPTION] = JsonUtil.JsonSerialize(loginfo);
                        }
                        else
                            await InformationAsync(loginfo.JobName, JsonUtil.JsonSerialize(loginfo), mailMessage);
                    }
                    catch (Exception)
                    {
                        await InformationAsync(loginfo.JobName, JsonUtil.JsonSerialize(loginfo), mailMessage);
                    }
                }


                //var gId = new Guid(context.JobDetail.Key.Name.Split('|')[0]);
                //// LogUtil.Debug("陈大猪。。。" + gId);
                ////LogUtil.Debug("ManagerJob Executing ...");
                //await _taskJobServices.ExcuteTaskJob(gId);
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                e2.RefireImmediately = true;

                stopwatch.Stop(); //  停止监视            
                double seconds = stopwatch.Elapsed.TotalSeconds;  //总秒数
                loginfo.ErrorMsg = $"<span class='error'>{ex.Message} {ex.StackTrace}</span>";
                context.JobDetail.JobDataMap[ConstantUtil.EXCEPTION] = JsonUtil.JsonSerialize(loginfo);
                loginfo.Seconds = seconds;
                await ErrorAsync(loginfo.JobName, ex, JsonUtil.JsonSerialize(loginfo), mailMessage);

            }
            finally
            {
                //LogUtil.Debug("ManagerJob Execute end ");

                logs.Add($"<p>{JsonUtil.JsonSerialize(loginfo)}</p>");
                context.JobDetail.JobDataMap[ConstantUtil.LOGLIST] = logs;
                double seconds = stopwatch.Elapsed.TotalSeconds;  //总秒数
                if (seconds >= warnTime)//如果请求超过20秒，记录警告日志    
                {
                    await WarningAsync(loginfo.JobName, "耗时过长 - " + JsonUtil.JsonSerialize(loginfo), mailMessage);
                }
            }

        }

        public async Task WarningAsync(string title, string msg, MailMessageEnum mailMessage)
        {
            LogUtil.Warn(msg);
            //if (mailMessage == MailMessageEnum.All)
            //{
            //    await new SetingController().SendMail(new Model.SendMailModel()
            //    {
            //        Title = $"任务调度-{title}【警告】消息",
            //        Content = msg
            //    });
            //}
        }

        public async Task InformationAsync(string title, string msg, MailMessageEnum mailMessage)
        {
            LogUtil.Info(msg);
            //if (mailMessage == MailMessageEnum.All)
            //{
            //    await new SetingController().SendMail(new Model.SendMailModel()
            //    {
            //        Title = $"任务调度-{title}消息",
            //        Content = msg
            //    });
            //}
        }

        public async Task ErrorAsync(string title, Exception ex, string msg, MailMessageEnum mailMessage)
        {
            LogUtil.Error(msg, ex);
            //if (mailMessage == MailMessageEnum.Err || mailMessage == MailMessageEnum.All)
            //{
            //    await new SetingController().SendMail(new Model.SendMailModel()
            //    {
            //        Title = $"任务调度-{title}【异常】消息",
            //        Content = msg
            //    });
            //}
        }


    }
}
