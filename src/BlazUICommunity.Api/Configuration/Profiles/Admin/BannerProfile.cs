using AutoMapper;
using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Api.Configuration.Profiles.Admin
{
    public class BannerProfile : Profile
    {
        public BannerProfile()
        {
            CreateMap<BannerDisplayDto, BzBannerModel>().IngoreNotMapped();
            CreateMap<BzBannerModel, BannerDisplayDto>().IngoreNotMapped();
        }
    }
}