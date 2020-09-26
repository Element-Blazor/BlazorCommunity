using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO.Admin;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles.Admin
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<TopicDisplayDto, BZTopicModel>().IngoreNotMapped();
            CreateMap<BZTopicModel, TopicDisplayDto>().IngoreNotMapped();
        }
    }
}