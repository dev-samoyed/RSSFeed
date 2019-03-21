using AutoMapper;
using RSSFeed.Data.Entities;
using RSSFeed.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSSFeed.Common.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<Post, PostModel>(MemberList.None).ReverseMap();
            CreateMap<Channel, ChannelModel>(MemberList.None).ReverseMap();
            CreateMap<Category, CategoryModel>(MemberList.None).ReverseMap();
        }
    }
}
