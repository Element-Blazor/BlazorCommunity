using AutoMapper;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;

namespace Blazui.Community.Api.Configuration.Profiles.Admin
{
    public class ReplyProfile : Profile
    {
        public ReplyProfile()
        {
            CreateMap<ReplyDisplayDto, BZReplyModel>().IngoreNotMapped();
            CreateMap<BZReplyModel, ReplyDisplayDto>().IngoreNotMapped();
        }
    }
}
