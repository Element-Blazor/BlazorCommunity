using AutoMapper;
using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Api.Configuration.Profiles.Admin
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