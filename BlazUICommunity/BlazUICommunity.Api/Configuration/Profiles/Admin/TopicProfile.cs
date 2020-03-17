using AutoMapper;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;
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
