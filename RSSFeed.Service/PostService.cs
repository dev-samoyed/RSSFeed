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
using System.Text.RegularExpressions;
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
            try
            {
                var post = _uow.GetRepository<Post>().All()
                        .FirstOrDefault(x => x.Title == postModel.Title && x.CreatedAt == postModel.CreatedAt);

                if (post != null)
                {
                    post.IsNew = false;
                    _uow.GetRepository<Post>().Update(post);
                    _uow.SaveChanges();
                    return;
                }

                post = _mapper.Map<Post>(postModel);
                post.Title = Regex.Replace(post.Title, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;|&mdash;", " ").Trim();
                post.Body = Regex.Replace(post.Body, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;|&mdash;", " ").Trim();

                _uow.GetRepository<Post>().Insert(post);
                _uow.SaveChanges();
            }
             catch (DbUpdateException ex)
            {
                throw new DbUpdateException("Duplicate", ex.InnerException);
            }
        }
        
        public async Task<PostModel> GetPostByIdAsync(Guid id)
        {
            var post = await _uow.GetRepository<Post>().All()
                                                        .Include(x => x.Channel)
                                                        .SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<PostModel>(post);
        }

        public async Task<IEnumerable<PostModel>> GetPosts(Guid channelId)
        {
            var posts = _uow.GetRepository<Post>().All().Include(x => x.Channel)
                                        .Where(x => x.ChannelId == channelId);
            return _mapper.Map<IEnumerable<PostModel>>(posts.OrderByDescending(post => post.CreatedAt));
        }

        public IDictionary<PostModel, CategoryModel> FeedItems(ChannelModel channel)
        {
            var items = new Dictionary<PostModel, CategoryModel>();
            var readerTask = FeedReader.ReadAsync(channel.Url);
            readerTask.ConfigureAwait(false);

            foreach (var item in readerTask.Result.Items)
            {
                var image = item.SpecificItem.Element.Descendants().ToList();

                var channelItem = new PostModel
                {
                    Channel = channel,
                    ChannelId = channel.Id,
                    Title = item.Title,
                    CreatedAt = item.PublishingDate.Value,
                    IsSeen = false,
                    IsNew = true,
                    PostUrl = item.Link,
                    Body = item.Description,
                    ImageUrl = image.FirstOrDefault(x => x.Name.LocalName.Contains("enclosure")) != null
                                ? item.SpecificItem.Element.Descendants().First(x => x.Name.LocalName == "enclosure").Attribute("url").Value
                                : channel.Image,
                    CategoryName = item.Categories.FirstOrDefault()
                };
                
                var category = _uow.GetRepository<Category>().All()
                        .FirstOrDefault(x => x.Name == item.Categories.FirstOrDefault() && x.ChannelId == channel.Id);

                var categoryModel = new CategoryModel();
                if (category == null)
                {
                    categoryModel.Name = item.Categories.FirstOrDefault();
                    categoryModel.ChannelId = channel.Id;
                }

                items.Add(channelItem, categoryModel);
            }

            return items;
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
            {
                return items.Where(x => x.Title.Contains(search.Value));
            }
            return items;
        }

        protected override IQueryable<Post> Category(IQueryable<Post> items, QuerySearch category)
        {
            if (!string.IsNullOrEmpty(category?.Value))
            {
                if (category.Value != "Все категории")
                {
                    var posts = items.Where(x => x.CategoryName.Contains(category.Value));
                    return posts;
                }
            }
            return items;
        }

        protected override IQueryable<Post> SourceOrder(IQueryable<Post> items, QuerySearch source)
        {
            if(Guid.TryParse(source.Value, out Guid result))
            {
                var posts = items.Where(x => x.ChannelId == Guid.Parse(source.Value));
                return posts;
            }
                

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

        public IEnumerable<PostModel> GetPosts()
        {
            var posts = _uow.GetRepository<Post>().All().Include(x => x.Channel);
            return _mapper.Map<IEnumerable<PostModel>>(posts.OrderByDescending(post => post.CreatedAt));
        }

    }
}
