using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.ReponseViewModel.TaskJob;
using NetCore.Services.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.IServices.I_TaskJob
{
    public interface ITaskJobServices :IBaseServices<TaskJobViewModel>
    {
        Task<HttpReponseViewModel> AddJob(Guid gId);
        Task<HttpReponseViewModel> StopJob(Guid gId);
        Task<HttpReponseViewModel> ResumeJob(Guid gId);

        Task<HttpReponseViewModel> DeleteJob(Guid gId);

        Task<bool> ExcuteTaskJob(Guid gId);

        Task JobSchedulerSetUp();

    }
}
