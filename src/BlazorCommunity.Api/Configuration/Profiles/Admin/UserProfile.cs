using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO.Admin;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDisplayDto, BZUserModel>().IngoreNotMapped();
            CreateMap<BZUserModel, UserDisplayDto>().IngoreNotMapped();
        }
    }
}