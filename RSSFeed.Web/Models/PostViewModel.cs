using RSSFeed.Service.Models;
using RSSFeed.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSSFeed.Web.Models
{
    public class PostViewModel
    {
        public IEnumerable<PostModel> Posts { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
