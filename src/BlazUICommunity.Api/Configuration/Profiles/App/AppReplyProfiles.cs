using AutoMapper;
using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Api.Configuration.Profiles.App
{
    public class AppReplyProfiles : Profile
    {
        public AppReplyProfiles()
        {
            CreateMap<BZReplyModel, PersonalReplyDisplayDto>().IngoreNotMapped();
        }
    }
}