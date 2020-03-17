using AutoMapper;
using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;

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