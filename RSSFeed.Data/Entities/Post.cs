using RSSFeed.Data.Entities.Base;
using System;
using System.Collections.Generic;

namespace RSSFeed.Data.Entities
{
    public class Post : EntityBase<Guid>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string PostUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ChannelId { get; set; }
        public virtual Channel Channel { get; set; }
        public bool IsNew { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSeen { get; set; }
    }
}
