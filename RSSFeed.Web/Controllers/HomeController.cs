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

        public IActionResult Index(string source, string sortType, int pageNumber = 1)
        {
            int pageSize = 10;

            var posts = _postService.GetPosts();
            GetFreshPosts(_channelService.GetChannels());
            ViewData["Sources"] = new SelectList(_channelService.GetChannels(), "Id", "Title");
            if (source != null && source != "Все")
            {
                var id = Guid.NewGuid();
                if (Guid.TryParse(source, out id))
                {
                    posts = posts.Where(p => p.ChannelId == Guid.Parse(source));
                    ViewData["Sources"] = new SelectList(_channelService.GetChannels(), "Id", "Title", Guid.Parse(source));
                }
                else
                    ViewData["Sources"] = new SelectList(_channelService.GetChannels(), "Id", "Title");
            }
            if (!String.IsNullOrEmpty(sortType))
            {
                posts = sortType == "date" ? posts.OrderByDescending(p => p.CreatedAt)
                                           : posts.OrderByDescending(p => p.Channel.Title);
            }

            var count = posts.Count();
            var items = posts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            PostViewModel viewModel = new PostViewModel
            {
                PageViewModel = new PageViewModel(count, pageNumber, pageSize),
                FilterViewModel = new FilterViewModel(source, sortType),
                Posts = items
            };

            
            ViewBag.SortType = sortType;
            return View(viewModel);
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
