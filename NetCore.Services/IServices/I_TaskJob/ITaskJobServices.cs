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
    public interface ITaskJobServices : IBaseServices<TaskJobViewModel>
    {

        Task<HttpReponseViewModel<string>> AddJob(Guid gId);
        Task<HttpReponseViewModel<string>> StopJob(Guid gId);

        Task<HttpReponseViewModel<string>> ResumeJob(Guid gId);
    }
}
