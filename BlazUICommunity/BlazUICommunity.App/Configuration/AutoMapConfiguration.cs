using AutoMapper;
using Blazui.Community.App.Model;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App
{
    public class AutoMapConfiguration : Profile
    {
        public AutoMapConfiguration()
        {
            CreateMap<BZUserModel, BZUserUIDto>();
            CreateMap<BZUserUIDto, BZUserModel>().ForMember(dest => dest.Id, option => option.Ignore());
            CreateMap<BZUserModel, BZUserDto>();
            CreateMap<BZUserDto, BZUserModel>().ForMember(dest => dest.Id, option => option.Ignore());
            CreateMap<BZTopicDto, PersonalTopicModel>();

            CreateMap<BZTopicDtoWithUser, TopicItemModel>()
                //.ForMember(target => target.ReplyCount, dto => dto.MapFrom(src => src.ReplyCount))
                //.ForMember(target => target.Avator, dto => dto.MapFrom(src => src.Avatar))
                //.ForMember(target => target.Title, dto => dto.MapFrom(src => src.Title))
                //.ForMember(target => target.TopicType, dto => dto.MapFrom(src => src.Category))

                .ForMember(target => target.Category, dto => dto.Ignore())
                .ForMember(target => target.ReleaseTime, dto => dto.MapFrom(src => src.PublishTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(target => target.Author, dto => dto.MapFrom(src => src.NickName))
                //.ForMember(target => target.Status, dto => dto.MapFrom(src => src.Status))
                .ForMember(target => target.IsBest, dto => dto.MapFrom(src => src.Good == 1));


            CreateMap<BZFollowDto, BZFollowModel>();
            CreateMap<BZFollowModel, BZFollowDto>();
        }
    }
}
