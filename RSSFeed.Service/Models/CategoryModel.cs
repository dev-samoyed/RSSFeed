using Newtonsoft.Json;
using RSSFeed.Service.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSSFeed.Service.Models
{
    public class CategoryModel : BaseModel
    {
        public string Name { get; set; }
        public Guid? ChannelId { get; set; }
        [JsonIgnore]
        public ChannelModel Channel { get; set; }
    }
}
