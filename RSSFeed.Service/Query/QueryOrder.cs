using RSSFeed.Service.Enums;

namespace RSSFeed.Service.Query
{
    public class QueryOrder<TOrderType>
    {
        public TOrderType OrderType { get; set; }
        public SortDirectionType Direction { get; set; }
        public int Order { get; set; }
    }
}
