using RSSFeed.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSSFeed.Data.Entities
{
    public class Category : EntityBase<Guid>
    {
        public string Name { get; set; }
        public Guid? ChannelId { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
