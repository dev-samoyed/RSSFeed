using RSSFeed.Service.Models.Base;

namespace RSSFeed.Service.Models
{
    public class ChannelModel : BaseModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
