using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles.App
{
    public class AppTopicProfiles : Profile
    {
        public AppTopicProfiles()
        {
            CreateMap<BZTopicModel, PersonalTopicDisplayDto>().IngoreNotMapped();
            CreateMap<BZTopicModel,HotTopicDto>().IngoreNotMapped();
        }
    }
}