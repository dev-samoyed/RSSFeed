using System.Collections.Generic;
using RSSFeed.Data.Enums;
using RSSFeed.Service.Models.Base;

namespace RSSFeed.Service.Models
{
    public class ChannelModel : BaseModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public ChannelType ChannelType { get; set; }
        public string Image { get; set; }
    }
}
