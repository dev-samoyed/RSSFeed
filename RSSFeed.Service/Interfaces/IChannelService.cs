using RSSFeed.Data.Entities;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Models;
using RSSFeed.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RSSFeed.Service.Interfaces
{
    public interface IChannelService : IBaseQueryService<Channel, ChannelModel, PostSortType>
    {
        IEnumerable<ChannelModel> GetChannels();
        void AddChannel(ChannelModel channel);
    }
}
