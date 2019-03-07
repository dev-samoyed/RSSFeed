using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var channels = _channelService.GetChannels();

            GetFreshPosts(channels);

            var posts = await GetPosts(pageSize, pageNumber);
            PageViewModel pageViewModel = new PageViewModel(posts.RecordsTotal, pageNumber, pageSize);
            PostViewModel postViewModel = new PostViewModel
            {
                PageViewModel = pageViewModel,
                Posts = posts.Data
            };
            ViewData["Sources"] = new SelectList(channels, "Id", "Title");
            return View(postViewModel);
        }

        public JsonResult PostSeen(string postId)
        {
            var id = Guid.Parse(postId);
            _postService.PostSeen(id);
            return Json(new { data = "success" });
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
