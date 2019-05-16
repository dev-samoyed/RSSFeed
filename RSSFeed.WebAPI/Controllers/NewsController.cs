using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Service.Query;

namespace RSSFeed.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        protected readonly IPostService _postService;
        protected readonly IChannelService _channelService;
        protected readonly ICategoryService _categoryService;
        protected readonly IMapper _mapper;

        public NewsController(IPostService postService, IChannelService channelService, ICategoryService categoryService, IMapper mapper)
        {
            _postService = postService;
            _channelService = channelService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        protected async Task<QueryResponse<PostModel>> GetPosts(int pageSize, int pageNumber, int sort, string category, string source, string query = null)
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
                        OrderType = sort == 0 ? PostSortType.PublishDate
                                              : PostSortType.ChannelTitle
                    }
                },
                Search = new QuerySearch
                {
                    Value = query
                },
                Category = new QuerySearch
                {
                    Value = category
                },
                SourceOrder = new QuerySearch
                {
                    Value = source
                }
            });
        }

        // GET api/news
        [HttpGet]
        public async Task<ActionResult<QueryResponse<PostModel>>> GetAll(int pageNumber, int sort, string category, string source, string query = null)
        {
            var pageSize = 40;
            var postModels = await GetPosts(pageSize, pageNumber, sort, category, source, (query ?? ""));
            return postModels;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
