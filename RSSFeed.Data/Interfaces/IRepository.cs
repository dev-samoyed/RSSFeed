using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RSSFeed.Data.Interfaces
{
    public interface IRepository { }

    public interface IRepository<T> : IRepository where T : class, IEntityBase, new()
    {
        IQueryable<T> All();
        Task<T> GetByIdAsync(params object[] id);
        T GetById(params object[] id);
        Task InsertAsync(T entity);
        void Insert(T entity);
        Task InsertAsync(IEnumerable<T> entities);
        void Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(params object[] id);
        void Delete(IEnumerable<T> entities);
    }
}
