using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO.Admin;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles.Admin
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