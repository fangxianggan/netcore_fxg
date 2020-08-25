using AutoMapper;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.DTO.TestModel;
using NetCore.EntityFrameworkCore.Models;

namespace NetCore.DTO.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public DomainToViewModelMappingProfile()
        {
            //CreateMap<Student, StudentViewModel>()
            //    .ForMember(d => d.County, o => o.MapFrom(s => s.Address.County))
            //    .ForMember(d => d.Province, o => o.MapFrom(s => s.Address.Province))
            //    .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
            //    .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            //    ;

            CreateMap<Test, TestViewModel>();


            CreateMap<StoreFiles, StoreFilesViewModel>()
                .ForMember(d => d.Uploader, o => o.MapFrom(s => s.CreateBy))
                .ForMember(d => d.UploadTime, o => o.MapFrom(s => s.CreateTime));


        }
    }
}
