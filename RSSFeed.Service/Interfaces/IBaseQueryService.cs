using RSSFeed.Data.Interfaces;
using RSSFeed.Service.Query;
using System.Threading.Tasks;

namespace RSSFeed.Services.Interfaces
{
    public interface IBaseQueryService<TEntity, TModel, TSortType>
        where TEntity : class, IEntityBase, new()
    {
        Task<QueryResponse<TModel>> GetAsync(QueryRequest<TSortType> query);
    }
}
