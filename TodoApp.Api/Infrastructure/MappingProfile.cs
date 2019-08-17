using AutoMapper;
using TodoApp.Api.ViewModels.Requests;
using TodoApp.Api.ViewModels.Responses;
using TodoApp.Common.Interface;
using TodoApp.DataAccess.Entities;

namespace TodoApp.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile(ICryptoHelper cryptoHelper)
        {
            // viewmodel -> entity
            CreateMap<NewUserRequest, User>()
                .ForMember(dest => dest.Password, src => src.MapFrom(x => cryptoHelper.Hash(x.Password)));

            // entity -> viewmodel
            CreateMap<User, UserViewModel>();
        }
    }
}
