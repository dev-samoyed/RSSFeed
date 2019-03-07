using RSSFeed.Data.Entities.Base;
using System;
using System.Collections.Generic;

namespace RSSFeed.Data.Entities
{
    public class Channel : EntityBase<Guid>
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public Channel()
        {
            Posts = new List<Post>();
        }
    }
}
