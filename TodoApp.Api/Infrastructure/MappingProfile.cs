using AutoMapper;
using TodoApp.Api.ViewModels;
using TodoApp.Api.ViewModels.Requests;
using TodoApp.Common.Interface;
using TodoApp.DataAccess.Entities;

namespace TodoApp.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile(ICryptoHelper cryptoHelper)
        {
            // viewmodel -> entity
            CreateMap<NewUserRequest, User>().ForMember(dest => dest.Password, src => src.MapFrom(x => cryptoHelper.Hash(x.Password)));
            
            CreateMap<ProjectTaskCreateViewModel, ProjectTask>().ReverseMap();
            CreateMap<ProjectTaskViewModel, ProjectTask>().ReverseMap();

            CreateMap<ProjectCreateViewModel, Project>().ReverseMap();
            CreateMap<ProjectViewModel, Project>().ReverseMap();
            CreateMap<ProjectAndTasksViewModel, Project>().ReverseMap();

            // entity -> viewmodel
            CreateMap<User, UserViewModel>();
        }
    }
}
