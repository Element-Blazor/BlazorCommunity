using AutoMapper;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;

namespace Blazui.Community.Api.Configuration.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDisplayDto, BZUserModel>().IngoreNotMapped();
            CreateMap<BZUserModel, UserDisplayDto>().IngoreNotMapped();
        }
    }
}
