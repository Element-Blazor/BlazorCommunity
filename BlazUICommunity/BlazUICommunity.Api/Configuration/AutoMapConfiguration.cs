using AutoMapper;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api
{
    public class AutoMapConfiguration : Profile
    {
        public AutoMapConfiguration()
        {
            CreateMap<BZUserModel, BZUserDto>();
            CreateMap<BZUserDto, BZUserModel>().ForMember(dest => dest.Id, option => option.Ignore());
            CreateMap<BZUserModel, BZUserUIDto>();
            CreateMap<BZUserUIDto, BZUserModel>().ForMember(dest => dest.Id, option => option.Ignore());
            CreateMap<BZTopicModel, BZTopicDto>();
            CreateMap<BZTopicDto, BZTopicModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<BZFollowModel, BZFollowDto>();
            CreateMap<BZFollowDto, BZFollowModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<BZPointModel, BZPointDto>();
            CreateMap<BZPointDto, BZPointModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<BZPointModel, BZPointDto>();
            CreateMap<BZPointDto, BZPointModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<BZReplyModel, BZReplyDto>();
            CreateMap<BZReplyDto, BZReplyModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<BZThirdAccountModel, BZThirdAccountDto>();
            CreateMap<BZThirdAccountDto, BZThirdAccountModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<BZUserRealVerificationModel, BZUserRealVerificationDto>();
            CreateMap<BZUserRealVerificationDto, BZUserRealVerificationModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<BZAddressModel, BZAddressDto>();
            CreateMap<BZAddressDto, BZAddressModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<SysLogModel, SysLogDto>();
            CreateMap<SysLogDto, SysLogModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<SysUserModel, SysUserDto>();
            CreateMap<SysUserDto, SysUserModel>().ForMember(dest => dest.Id, option => option.Ignore());

            //CreateMap<BZTopicDto, BZTopicDtoWithUser>()
            //    .ForMember(dest => dest.UserName, option => option.Ignore())
            //    .ForMember(dest => dest.Avatar, option => option.Ignore())
            //    .ForMember(dest => dest.NickName, option => option.Ignore());

            CreateMap<BZTopicModel, BZTopicDtoWithUser>()
            .ForMember(dest => dest.UserName, option => option.Ignore())
            .ForMember(dest => dest.Avator, option => option.Ignore())
            .ForMember(dest => dest.NickName, option => option.Ignore())
             .ForMember(dest => dest.UserId, option => option.Ignore())
             .ForMember(dest => dest.GoodDisplay, option => option.Ignore())
              .ForMember(dest => dest.TopDisplay, option => option.Ignore())
             .ForMember(dest => dest.StatusDisplay, option => option.Ignore());


            CreateMap<BZReplyModel, BZReplyDtoWithUser>()
             .ForMember(dest => dest.UserName, option => option.Ignore())
             .ForMember(dest => dest.Avator, option => option.Ignore())
             .ForMember(dest => dest.NickName, option => option.Ignore())
             .ForMember(dest => dest.StatusDisplay, option => option.Ignore())
              .ForMember(dest => dest.UserId, option => option.Ignore());

        }
    }
}
