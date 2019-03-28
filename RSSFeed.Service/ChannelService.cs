using AutoMapper;
using RSSFeed.Data.Entities;
using RSSFeed.Data.Interfaces;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Service.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFeed.Service
{
    public class ChannelService : BaseQueryService<Channel, ChannelModel, PostSortType>, IChannelService
    {
        public ChannelService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        public void AddChannel(ChannelModel channelModel)
        {
            var channel = _uow.GetRepository<Channel>().All()
                        .FirstOrDefault(x => x.Title == channelModel.Title && x.Url == channelModel.Url);

            if (channel != null)
                return;

            channel = _mapper.Map<Channel>(channelModel);

            _uow.GetRepository<Channel>().Insert(channel);

            _uow.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var channel = _uow.GetRepository<Channel>().GetById(id);
            _uow.GetRepository<Channel>().Delete(channel);
            _uow.SaveChanges();
        }

        public ChannelModel GetById(Guid id)
        {
            var channel = _uow.GetRepository<Channel>().GetById(id);
            return _mapper.Map<ChannelModel>(channel);
        }

        public IEnumerable<ChannelModel> GetChannels()
        {
            var channels = _uow.GetRepository<Channel>().All();
            return _mapper.Map<IEnumerable<ChannelModel>>(channels.OrderBy(title => title.Title));
        }

        protected override IQueryable<Channel> Category(IQueryable<Channel> items, QuerySearch category)
        {
            return items;
        }

        protected override IQueryable<Channel> Order(IQueryable<Channel> items, bool isFirst, QueryOrder<PostSortType> order)
        {
            return items;
        }

        protected override IQueryable<Channel> Search(IQueryable<Channel> items, QuerySearch search)
        {
            return items;
        }

        protected override IQueryable<Channel> SourceOrder(IQueryable<Channel> items, QuerySearch source)
        {
            return items;
        }
    }
}
