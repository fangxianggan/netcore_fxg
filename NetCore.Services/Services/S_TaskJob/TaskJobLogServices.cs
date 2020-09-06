using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.TaskJob;
using NetCore.EntityFrameworkCore.Models;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.IServices.I_TaskJob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_TaskJob
{
    public class TaskJobLogServices : BaseServices<TaskJobLog, TaskJobLogViewModel>, ITaskJobLogServices
    {
        private readonly IBaseDomain<TaskJobLog> _baseDomian;
        public TaskJobLogServices(IBaseDomain<TaskJobLog> baseDomian) : base(baseDomian)
        {
            _baseDomian = baseDomian;
        }
    }
}
