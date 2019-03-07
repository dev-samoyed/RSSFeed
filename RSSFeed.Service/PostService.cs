using AutoMapper;
using CodeHollow.FeedReader;
using Microsoft.EntityFrameworkCore;
using RSSFeed.Data.Entities;
using RSSFeed.Data.Interfaces;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Extensions;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Service.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSSFeed.Service
{
    public class PostService : BaseQueryService<Post, PostModel, PostSortType>, IPostService
    {
        public PostService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        public void AddPost(PostModel postModel)
        {
            var post = _uow.GetRepository<Post>().All()
                        .FirstOrDefault(x => x.Title == postModel.Title && x.CreatedAt == postModel.CreatedAt);

            if (post != null)
                return;

            post = _mapper.Map<Post>(postModel);
            _uow.GetRepository<Post>().Insert(post);
            
            _uow.SaveChanges();
        }
        
        public async Task<PostModel> GetPostByIdAsync(Guid id)
        {
            var post = await _uow.GetRepository<Post>().All()
                                                        .Include(x => x.Channel)
                                                        .SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<PostModel>(post);
        }

        public IEnumerable<PostModel> GetPosts()
        {
            var posts = _uow.GetRepository<Post>().All();
            return _mapper.Map<IEnumerable<PostModel>>(posts.OrderBy(title => title.Title));
        }

        public IList<PostModel> FeedItems(ChannelModel channel)
        {
            var postModels = new List<PostModel>();
            var readerTask = FeedReader.ReadAsync(channel.Url);
            readerTask.ConfigureAwait(false);

            foreach (var item in readerTask.Result.Items)
            {
                var channelItem = new PostModel
                {
                    Channel = channel,
                    ChannelId = channel.Id,
                    Title = item.Title,
                    CreatedAt = item.PublishingDate.Value,
                    IsSeen = false,
                    PostUrl = item.Link,
                    Body = item.Content
                };
                postModels.Add(channelItem);
            }

            return postModels;
        }

        protected override IQueryable<Post> Order(IQueryable<Post> items, bool isFirst, QueryOrder<PostSortType> order)
        {
            switch (order.OrderType)
            {
                case PostSortType.ChannelTitle:
                    return items.OrderWithDirectionBy(isFirst, order.Direction, x => x.Channel.Title);
                case PostSortType.PublishDate:
                    return items.OrderWithDirectionBy(isFirst, order.Direction, x => x.CreatedAt);
            }

            throw new ArgumentOutOfRangeException(nameof(order.OrderType));
        }

        protected override IQueryable<Post> Paging(IQueryable<Post> items, int? start, int? length)
        {
            return items.Skip(start.Value).Take(length.Value);
        }

        protected override IQueryable<Post> Search(IQueryable<Post> items, QuerySearch search)
        {
            if (!string.IsNullOrEmpty(search?.Value))
                return items.Where(x => x.Title.ToLower().Contains(search.Value.ToLower())).OrderBy(post => post.Title);
            return items;
        }

        public void PostSeen(Guid postId)
        {
            var post = _uow.GetRepository<Post>().GetById(postId);
            if (post == null)
                return;

            post.IsSeen = true;
            _uow.GetRepository<Post>().Update(post);
            _uow.SaveChanges();
        }
    }
}
