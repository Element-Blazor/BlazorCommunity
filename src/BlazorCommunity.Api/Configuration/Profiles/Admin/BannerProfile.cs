using AutoMapper;
using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DTO.Admin;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Api.Configuration.Profiles.Admin
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