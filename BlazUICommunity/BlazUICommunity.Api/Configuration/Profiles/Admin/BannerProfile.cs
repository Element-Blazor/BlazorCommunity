using AutoMapper;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;

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
