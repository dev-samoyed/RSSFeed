using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RSSFeed.Service.Enums;
using RSSFeed.Service.Interfaces;
using RSSFeed.Service.Query;

namespace RSSFeed.Web.Areas.Admin.Controllers
{
    public class ChannelsController : BaseController
    {
        public ChannelsController(IChannelService channelService, IMapper mapper) : base(channelService, mapper)
        {
        }

        public IActionResult Index(string query = null)
        {
            
            return View();
        }
    }
}