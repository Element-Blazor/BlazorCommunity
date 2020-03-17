using AutoMapper;
using Blazui.Community.App.Model;
using Blazui.Community.DateTimeExtensions;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;

namespace Blazui.Community.App
{
    public class AutoMapConfiguration : Profile
    {
        public AutoMapConfiguration()
        {
            CreateMap<BZUserModel, BZUserDto>().ForMember(dest => dest.StatusDisplay, option => option.Ignore());
            CreateMap<BZUserDto, BZUserModel>();

            CreateMap<BZTopicModel, BZTopicDto>()
                  .ForMember(dest => dest.StatusDisplay, option => option.Ignore())
            .ForMember(dest => dest.UserName, option => option.Ignore())
            .ForMember(dest => dest.Avator, option => option.Ignore())
            .ForMember(dest => dest.NickName, option => option.Ignore())
             //.ForMember(dest => dest.UserId, option => option.Ignore())
             .ForMember(dest => dest.GoodDisplay, option => option.Ignore())
              .ForMember(dest => dest.TopDisplay, option => option.Ignore())
             .ForMember(dest => dest.StatusDisplay, option => option.Ignore());

            CreateMap<BZTopicDto, PersonalTopicModel>()
               .ForMember(dest => dest.StatusDisplay, option => option.Ignore());

            CreateMap<BZTopicDto, BZTopicModel>();

            CreateMap<BZTopicDto, TopicFormModel>().ForMember(t => t.ReleaseTime, op => op.MapFrom(s => s.CreateDate.ConvertToDateDiffStr()));

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
             .ForMember(dest => dest.StatusDisplay, option => option.Ignore())
               .ForMember(dest => dest.LastModifyDateDisplay, option => option.Ignore());
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
        }
    }
}