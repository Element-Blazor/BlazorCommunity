using AutoMapper;
using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Api.Configuration.Profiles.Admin
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