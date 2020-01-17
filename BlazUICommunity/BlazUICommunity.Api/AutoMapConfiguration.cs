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
            CreateMap<BZUserModel , BZUserDto>().AfterMap((source,dest)=>dest.Cypher="*********");
            CreateMap<BZUserDto , BZUserModel>().ForMember(dest => dest.Id , option => option.Ignore());
        }
    }
}
