using RSSFeed.Data.Entities;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Models;
using RSSFeed.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RSSFeed.Service.Interfaces
{
    public interface IPostService : IBaseQueryService<Post, PostModel, PostSortType>
    {
        Task<IEnumerable<PostModel>> GetPosts(Guid channelId);
        IEnumerable<PostModel> GetPosts();
        Task<PostModel> GetPostByIdAsync(Guid id);
        void AddPost(PostModel postModel);
        IList<PostModel> FeedItems(ChannelModel channel);
        void PostSeen(Guid postId);
    }
}
