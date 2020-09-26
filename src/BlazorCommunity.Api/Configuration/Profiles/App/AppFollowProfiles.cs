using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles.App
{
    public class AppFollowProfiles : Profile
    {
        public AppFollowProfiles()
        {
            CreateMap<BZTopicModel, PersonalFollowDisplayDto>().IngoreNotMapped();
        }
    }
}