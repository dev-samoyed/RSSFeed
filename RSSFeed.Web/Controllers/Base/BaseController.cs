using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Service.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RSSFeed.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        protected readonly IPostService _postService;
        protected readonly IChannelService _channelService;
        protected readonly IMapper _mapper;

        public BaseController(IPostService postService, IChannelService channelService, IMapper mapper)
        {
            _postService = postService;
            _channelService = channelService;
            _mapper = mapper;
        }

        protected async Task<QueryResponse<PostModel>> GetPosts(int pageSize, int pageNumber, string query = null)
        {
            return await _postService.GetAsync(new QueryRequest<PostSortType>
            {
                Start = (pageSize * (pageNumber - 1)),
                Length = pageSize,
                Includes = new[]
                {
                    "Channel"
                },
                OrderQueries = new[]
                {
                    new QueryOrder<PostSortType>
                    {
                        Direction = SortDirectionType.Descending,
                        OrderType = PostSortType.PublishDate
                    }
                },
                Search = new QuerySearch
                {
                    Value = query
                }
            });
        }
    }
}