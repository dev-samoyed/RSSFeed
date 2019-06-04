using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Service.Query;
using System;
using System.Threading.Tasks;

namespace RSSFeed.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ChannelsController : BaseController
    {
        public ChannelsController(IChannelService channelService, IMapper mapper) : base(channelService, mapper)
        {
        }

        public IActionResult Index(string query = null)
        {
            return View();
        }

        public async Task<JsonResult> GetData()
        {
            var channels = await _channelService.GetAsync(new QueryRequest<PostSortType>
            {
                Includes = new[]
                {
                    "Categories"
                },
                OrderQueries = new[]
                {
                    new QueryOrder<PostSortType>
                    {
                        Direction = SortDirectionType.Descending,
                        OrderType = PostSortType.ChannelTitle
                    }
                }
            });
            return Json(new { channels.Data });
        }
        
        public JsonResult Create(string imageUrl, string title, string url)
        {
            try
            {
                var channel = new ChannelModel()
                {
                    Image = imageUrl,
                    Url = url,
                    Title = title
                };
                _channelService.AddChannel(channel);
                return Json(new { data = "success" });
            }
            catch (Exception)
            {
                return Json(new { data = "failed" });
            }
        }

        public JsonResult Delete(string id)
        {
            try
            {
                var testId = Guid.NewGuid();
                if (Guid.TryParse(id, out testId))
                    _channelService.Delete(Guid.Parse(id));
                return Json(new { data = "success" });
            }
            catch (Exception)
            {
                return Json(new { data = "failed" });
            }
        }
    }
}