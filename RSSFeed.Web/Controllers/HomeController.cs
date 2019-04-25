using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Web.Controllers.Base;
using RSSFeed.Web.Models;
using RSSFeed.Web.Util;

namespace RSSFeed.Web.Controllers
{
    public class HomeController : BaseController
    {
        protected readonly IHubContext<NewsHub> _hubContext;
        public HomeController(IPostService postService, IChannelService channelService, ICategoryService categoryService, IMapper mapper, IHubContext<NewsHub> hubContext) 
            : base(postService, channelService, categoryService, mapper) { _hubContext = hubContext; }

        public IActionResult Index(string query)
        {
            ViewBag.SearchQuery = (query ?? "");
            ViewBag.Sources = new SelectList(_channelService.GetChannels(), "Id", "Title");

            RecurringJob.AddOrUpdate(
                    () => RunInBackground(),
                    Cron.MinuteInterval(5));
            return View();
        }

        // method to getting data to scrolling page
        public async Task<JsonResult> GetData(int pageNumber, string query, string source, int sort, string category)
        {
            var pageSize = 40;
            var postModels = await GetPosts(pageSize, pageNumber, sort, category, source, query);
            
            var exampleId = Guid.NewGuid();
            if (Guid.TryParse(source, out exampleId) && category != "Все категории")
            {
                ViewBag.Sources = new SelectList(_channelService.GetChannels(), "Id", "Title", Guid.Parse(source));
            }
            else
            {
                ViewBag.Sources = new SelectList(_channelService.GetChannels(), "Id", "Title");
            }
            
            return Json(new { postModels.Data, total = postModels.RecordsTotal, filtered = postModels.RecordsFiltered });
        }

        public JsonResult GetCategoriesBySource(Guid sourceId)
        {
            var categories = new List<CategoryModel>();
            categories = _categoryService.GetAllCategories(sourceId).ToList();
            categories.Insert(0, (new CategoryModel { Name = "Все категории"}));
            return Json(new SelectList(categories, "Name", "Name"));
        }
        
        public JsonResult PostSeen(string postId)
        {
            var id = Guid.Parse(postId);
            _postService.PostSeen(id);
            return Json(new { data = "success" });
        }

        public void RunInBackground()
        {
            // add channels, if not exist
            var channels = GetChannels();
            
            foreach (var channel in channels)
            {
                _channelService.AddChannel(channel);
            }

            var channelModels = _channelService.GetChannels();
            foreach (var channel in channelModels)
            {
                var feedItems = _postService.FeedItems(channel);
                foreach (KeyValuePair<PostModel, CategoryModel> keyValuePair in feedItems)
                {
                    keyValuePair.Key.Title = Regex.Replace(keyValuePair.Key.Title, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;|&mdash;", " ").Trim();
                    keyValuePair.Key.Body = Regex.Replace(keyValuePair.Key.Body, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;|&mdash;", " ").Trim();
                    try
                    {
                        //add post
                        _postService.AddPost(keyValuePair.Key);
                        //add category
                        _categoryService.AddCategories(keyValuePair.Value, channel.Id);
                    }
                    catch (DbUpdateException ex)
                    {
                        continue;
                    }
                }
            }

            _hubContext.Clients.All.SendAsync("broadcastMessage", _postService.GetPosts().Where(x=>x.IsNew).Count());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IList<ChannelModel> GetChannels()
        {
            return new List<ChannelModel>
            {
                new ChannelModel
                {
                    Title = "K-News",
                    Url = "https://knews.kg/feed/",
                    Image = "https://knews.kg/wp-content/uploads/2016/02/logo.png"
                },
                new ChannelModel
                {
                    Title = "Habr",
                    Url = "http://habrahabr.ru/rss/",
                    Image = "https://habr.com/images/habr.png"
                },
                new ChannelModel
                {
                    Title = "24kg",
                    Url = "https://24.kg/rss/",
                    Image = "https://24.kg/assets/42adfee/images/logo.png"
                },
                new ChannelModel
                {
                    Title = "Sputnik Бишкек",
                    Url = "https://sputnik.kg/export/rss2/archive/index.xml",
                    Image = "https://ru.sputnik.kg/i/logo.png"
                },
                new ChannelModel
                {
                    Title = "Kaktus Media",
                    Url = "https://kaktus.media/?rss",
                    Image = "https://kaktus.media/lenta4/static/img/logo.png?2"
                }
            };
        }
    }
}
