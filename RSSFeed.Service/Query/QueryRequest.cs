using System.Collections.Generic;

namespace RSSFeed.Service.Query
{
    public class QueryRequest<TSortType>
    {
        public int? Start { get; set; }
        public int? Length { get; set; }
        public QuerySearch Search { get; set; }
        public IEnumerable<QueryOrder<TSortType>> OrderQueries { get; set; }
        public IEnumerable<string> Includes { get; set; }
        public QuerySearch Category { get; set; }
        public QuerySearch SourceOrder { get; set; }
    }
}
