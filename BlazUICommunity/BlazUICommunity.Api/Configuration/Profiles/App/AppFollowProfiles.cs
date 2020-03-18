using AutoMapper;
using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Configuration.Profiles.App
{
    public class AppFollowProfiles : Profile
    {
        public AppFollowProfiles()
        {
            CreateMap<BZTopicModel, PersonalFollowDisplayDto>().IngoreNotMapped();
        }
    }
}