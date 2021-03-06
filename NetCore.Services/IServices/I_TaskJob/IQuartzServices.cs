﻿using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_TaskJob
{
    public interface IQuartzServices
    {
        Task<bool> Add(Type type, JobDataMap keyValues, JobKey jobKey, ITrigger trigger);
        Task<bool> Resume(JobKey jobKey);
        Task<bool> Stop(JobKey jobKey);
        Task<bool> Delete(JobKey jobKey);
        void AddJobListener();
        Task StartScheduleAsync();
       
    }
}
