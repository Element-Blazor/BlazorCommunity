using AutoMapper;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;
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
            CreateMap<BZUserModel, BZUserDto>().IngoreNotMapped();
            CreateMap<BZUserDto, BZUserModel>();

            CreateMap<BZTopicModel, SeachTopicDto>();
            CreateMap<BZTopicModel, BZTopicDto>();
            CreateMap<BZTopicDto, BZTopicModel>();

            CreateMap<BZFollowModel, BZFollowDto>().IngoreNotMapped();
            CreateMap<BZFollowDto, BZFollowModel>();

            CreateMap<BZPointModel, BZPointDto>().IngoreNotMapped();
            CreateMap<BZPointDto, BZPointModel>();

            CreateMap<BZPointModel, BZPointDto>().IngoreNotMapped();
            CreateMap<BZPointDto, BZPointModel>();

            CreateMap<BZReplyModel, BZReplyDto>().IngoreNotMapped();
            CreateMap<BZReplyDto, BZReplyModel>();

            CreateMap<BZAutho2Model, BZOauthDto>().IngoreNotMapped();
            CreateMap<BZOauthDto, BZAutho2Model>();

            CreateMap<BZReplyModel, BZReplyDto>().IngoreNotMapped();
            CreateMap<BZReplyDto, BZReplyModel>();

            CreateMap<BzVerifyCodeModel, BzVerifyCodeDto>().IngoreNotMapped();
            CreateMap<BzVerifyCodeDto, BzVerifyCodeModel>();

            CreateMap<BZVersionModel, BZVersionDto>().IngoreNotMapped();
            CreateMap<BZVersionDto, BZVersionModel>();

            CreateMap<BzBannerModel, BzBannerDto>().IngoreNotMapped();
            CreateMap<BzBannerDto, BzBannerModel>();

            CreateMap<BZAddressModel, BZAddressDto>().IngoreNotMapped();
            CreateMap<BZAddressDto, BZAddressModel>();

            CreateMap<SysLogModel, SysLogDto>().IngoreNotMapped();
            CreateMap<SysLogDto, SysLogModel>();

            CreateMap<SysUserModel, SysUserDto>().IngoreNotMapped();
            CreateMap<SysUserDto, SysUserModel>();

            CreateMap<BZIDCardModel, BZIDCardDto>().IngoreNotMapped();
            CreateMap<BZIDCardDto, BZIDCardModel>();


        }
    }
}
