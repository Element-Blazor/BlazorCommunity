using AutoMapper;
using Blazui.Community.Admin.Data;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Admin.AutoConfiguration
{
    public class AutoMapConfiguration : Profile
    {
        public AutoMapConfiguration()
        {
            CreateMap<BZUserModel, BZUserUIDto>()
                .ForMember(dest => dest.StatusDisplay, option => option.Ignore())
                .ForMember(dest => dest.SexDisplay, option => option.Ignore());
            CreateMap<BZUserUIDto, BZUserModel>().ForMember(dest => dest.Id, option => option.Ignore());
            CreateMap<BZUserModel, BZUserDto>();
            CreateMap<BZUserDto, BZUserModel>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<VersionModel, BZVersionModel>();
            CreateMap<BZVersionModel, VersionModel>();
        }
    }
}
