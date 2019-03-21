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
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Models;
using RSSFeed.Web.Controllers.Base;
using RSSFeed.Web.Models;
using RSSFeed.Web.Util;

namespace RSSFeed.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IPostService postService, IChannelService channelService, IMapper mapper) 
            : base(postService, channelService, mapper) { }

        public IActionResult Index(string query)
        {
            ViewBag.SearchQuery = (query ?? "");
            RecurringJob.AddOrUpdate(
                    () => RunInBackground(),
                    Cron.MinuteInterval(1));
            return View();
        }

        // method to getting data to scrolling page
        public async Task<JsonResult> GetData(int pageNumber, string query)
        {
            var pageSize = 40;
            var postModels = await GetPosts(pageSize, pageNumber, query);

            return Json(new { postModels.Data, total = postModels.RecordsTotal, filtered = postModels.RecordsFiltered });
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
            var channels = new List<ChannelModel>
            {
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

            foreach (var channel in channels)
            {
                _channelService.AddChannel(channel);
            }

            var channelModels = _channelService.GetChannels();
            foreach (var channel in channelModels)
            {
                var feedItems = _postService.FeedItems(channel);
                foreach (var channelItem in feedItems)
                {
                    channelItem.Title = Regex.Replace(channelItem.Title, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;|&mdash;", " ").Trim();
                    channelItem.Body = Regex.Replace(channelItem.Body, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;|&mdash;", " ").Trim();
                    _postService.AddPost(channelItem);
                }
            }
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void GetFreshPosts(IEnumerable<ChannelModel> channelModels)
        {
            foreach (var channel in channelModels)
            {
                var feedItems = _postService.FeedItems(channel);
                foreach (var channelItem in feedItems)
                {
                    _postService.AddPost(channelItem);
                }
            }
        }
    }
}
