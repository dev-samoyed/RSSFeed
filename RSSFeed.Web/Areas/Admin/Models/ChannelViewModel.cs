using RSSFeed.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSSFeed.Web.Areas.Admin.Models
{
    public class ChannelViewModel
    {
        public IList<ChannelModel> Channels { get; set; }
    }
}
