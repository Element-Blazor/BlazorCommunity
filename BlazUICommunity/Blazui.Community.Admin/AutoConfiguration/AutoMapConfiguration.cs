using AutoMapper;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Admin.AutoConfiguration
{
    public class AutoMapConfiguration : Profile
    {
        public AutoMapConfiguration()
        {
            CreateMap<BZUserModel, BZUserDto>().ForMember(dest => dest.StatusDisplay, option => option.MapFrom(src => ((DelStatus)src.Status).Description()))
                .ForMember(dest => dest.SexDisplay, option => option.MapFrom(src => src.Sex == 0 ? "男" : "女"));
            CreateMap<BZUserDto, BZUserModel>();

            CreateMap<BZTopicModel, BZTopicDto>()
            .ForMember(dest => dest.UserName, option => option.Ignore())
            .ForMember(dest => dest.Avator, option => option.Ignore())
            .ForMember(dest => dest.NickName, option => option.Ignore())
            .ForMember(dest => dest.GoodDisplay, option => option.Ignore())
            .ForMember(dest => dest.TopDisplay, option => option.Ignore())
            .ForMember(dest => dest.StatusDisplay, option => option.MapFrom(src => ((DelStatus)src.Status).Description()));

            CreateMap<BZTopicDto, BZTopicModel>();



            CreateMap<BZFollowModel, BZFollowDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZFollowDto, BZFollowModel>();

            CreateMap<BZPointModel, BZPointDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZPointDto, BZPointModel>();

            CreateMap<BZPointModel, BZPointDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZPointDto, BZPointModel>();

            CreateMap<BZReplyModel, BZReplyDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZReplyDto, BZReplyModel>();

            CreateMap<BZAutho2Model, BZOauthDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZOauthDto, BZAutho2Model>();

            CreateMap<BZReplyModel, BZReplyDto>()
              .ForMember(dest => dest.UserName, option => option.Ignore())
            .ForMember(dest => dest.Avator, option => option.Ignore())
            .ForMember(dest => dest.NickName, option => option.Ignore())
             .ForMember(dest => dest.UserId, option => option.Ignore())
             .ForMember(dest => dest.GoodDisplay, option => option.Ignore())
              .ForMember(dest => dest.TopDisplay, option => option.Ignore())
             .ForMember(dest => dest.StatusDisplay, option => option.MapFrom(src => ((DelStatus)src.Status).Description()))
               .ForMember(dest => dest.LastModifyDateDisplay, option => option.Ignore());
            CreateMap<BZReplyDto, BZReplyModel>();

            CreateMap<BzVerifyCodeModel, BzVerifyCodeDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BzVerifyCodeDto, BzVerifyCodeModel>();

            CreateMap<BZVersionModel, BZVersionDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZVersionDto, BZVersionModel>();
            CreateMap<BZVersionDto, VersionAutoGenerateColumnsDto>().ForMember(dest => dest.ProjectDisplay, option => option.Ignore());

            CreateMap<BzBannerModel, BzBannerDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BzBannerDto, BzBannerModel>();

            CreateMap<BzBannerDto, BannerAutoGenerateColumnsDto>().ForMember(dest => dest.Previews, option => option.Ignore());
            CreateMap<BannerAutoGenerateColumnsDto, BzBannerDto>();
            CreateMap<BZAddressModel, BZAddressDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZAddressDto, BZAddressModel>();

            CreateMap<SysLogModel, SysLogDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<SysLogDto, SysLogModel>();

            CreateMap<SysUserModel, SysUserDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<SysUserDto, SysUserModel>();

            CreateMap<BZIDCardModel, BZIDCardDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZIDCardDto, BZIDCardModel>();
        }

        string test(string s)
        {
            return s;
        }
    }
}