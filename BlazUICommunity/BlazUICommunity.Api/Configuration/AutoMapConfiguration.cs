using AutoMapper;
using BlazUICommunity.DTO;
using BlazUICommunity.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazUICommunity.Api
{
    public class AutoMapConfiguration: Profile
    {
        public AutoMapConfiguration()
        {
            CreateMap<BZUserModel , BZUserDto>().AfterMap((source,dest)=>dest.IdentityUser="*********");
            CreateMap<BZUserDto , BZUserModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZTopicModel , BZTopicDto>();
            CreateMap<BZTopicDto , BZTopicModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZFollowModel , BZFollowDto>();
            CreateMap<BZFollowDto , BZFollowModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZPointModel , BZPointDto>();
            CreateMap<BZPointDto , BZPointModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZPointModel , BZPointDto>();
            CreateMap<BZPointDto , BZPointModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZReplyModel , BZReplyDto>();
            CreateMap<BZReplyDto , BZReplyModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZThirdAccountModel , BZThirdAccountDto>();
            CreateMap<BZThirdAccountDto , BZThirdAccountModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZUserRealVerificationModel , BZUserRealVerificationDto>();
            CreateMap<BZUserRealVerificationDto , BZUserRealVerificationModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<BZAddressModel , BZAddressDto>();
            CreateMap<BZAddressDto , BZAddressModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<SysLogModel , SysLogDto>();
            CreateMap<SysLogDto , SysLogModel>().ForMember(dest => dest.Id , option => option.Ignore());

            CreateMap<SysUserModel , SysUserDto>();
            CreateMap<SysUserDto , SysUserModel>().ForMember(dest => dest.Id , option => option.Ignore());
        }
    }
}
