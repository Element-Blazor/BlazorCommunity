using AutoMapper;
using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Api
{
    /// <summary>
    ///
    /// </summary>
    public class AutoMapProfiles : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public AutoMapProfiles()
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

        

            CreateMap<BZIDCardModel, BZIDCardDto>().IngoreNotMapped();
            CreateMap<BZIDCardDto, BZIDCardModel>();
        }
    }
}