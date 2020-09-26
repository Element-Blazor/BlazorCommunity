using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles.App
{
    public class AppReplyProfiles : Profile
    {
        public AppReplyProfiles()
        {
            CreateMap<BZReplyModel, PersonalReplyDisplayDto>().IngoreNotMapped();
        }
    }
}