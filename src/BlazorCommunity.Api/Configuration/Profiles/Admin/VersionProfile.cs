using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO.Admin;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles.Admin
{
    public class VersionProfile : Profile
    {
        public VersionProfile()
        {
            CreateMap<VersionDisplayDto, BZVersionModel>().IngoreNotMapped();
            CreateMap<BZVersionModel, VersionDisplayDto>().IngoreNotMapped();
        }
    }
}