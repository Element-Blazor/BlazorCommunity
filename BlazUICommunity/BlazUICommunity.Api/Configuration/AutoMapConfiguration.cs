using AutoMapper;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapConfiguration : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapConfiguration()
        {
            CreateMap<BZUserModel, BZUserDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZUserDto, BZUserModel>();

            CreateMap<BZTopicModel, SeachTopicDto>();
            CreateMap<BZTopicModel, BZTopicDto>()
                  .ForMember(dest => dest.StatusDisplay, option => option.Ignore())
            .ForMember(dest => dest.UserName, option => option.Ignore())
            .ForMember(dest => dest.Avator, option => option.Ignore())
            .ForMember(dest => dest.NickName, option => option.Ignore())
             //.ForMember(dest => dest.UserId, option => option.Ignore())
             .ForMember(dest => dest.GoodDisplay, option => option.Ignore())
              .ForMember(dest => dest.TopDisplay, option => option.Ignore())
             .ForMember(dest => dest.StatusDisplay, option => option.Ignore());
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
                .ForMember(dest => dest.StatusDisplay, option => option.Ignore())
              .ForMember(dest => dest.UserName, option => option.Ignore())
            .ForMember(dest => dest.Avator, option => option.Ignore())
            .ForMember(dest => dest.NickName, option => option.Ignore())
             .ForMember(dest => dest.UserId, option => option.Ignore())
             .ForMember(dest => dest.GoodDisplay, option => option.Ignore())
              .ForMember(dest => dest.TopDisplay, option => option.Ignore())
             .ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZReplyDto, BZReplyModel>();

            CreateMap<BzVerifyCodeModel, BzVerifyCodeDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BzVerifyCodeDto, BzVerifyCodeModel>();

            CreateMap<BZVersionModel, BZVersionDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZVersionDto, BZVersionModel>();

            CreateMap<BzBannerModel, BzBannerDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BzBannerDto, BzBannerModel>();

            CreateMap<BZAddressModel, BZAddressDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZAddressDto, BZAddressModel>();

            CreateMap<SysLogModel, SysLogDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<SysLogDto, SysLogModel>();

            CreateMap<SysUserModel, SysUserDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<SysUserDto, SysUserModel>();

            CreateMap<BZIDCardModel, BZIDCardDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZIDCardDto, BZIDCardModel>();

            //CreateMap<BZTopicModel, BZTopicDtoWithUser>()


            //CreateMap<BZReplyModel, BZReplyDtoWithUser>()
            // .ForMember(dest => dest.UserName, option => option.Ignore())
            // .ForMember(dest => dest.Avator, option => option.Ignore())
            // .ForMember(dest => dest.NickName, option => option.Ignore())
            // .ForMember(dest => dest.StatusDisplay, option => option.Ignore())
            //  .ForMember(dest => dest.UserId, option => option.Ignore());

        }
    }
}
