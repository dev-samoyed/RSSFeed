using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RSSFeed.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RSSFeed.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntityBase, new()
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> All() =>
            _context.Set<T>();

        #region Async

        public virtual Task<T> GetByIdAsync(params object[] id) =>
            _context.FindAsync<T>(id);

        public virtual Task InsertAsync(T entity) =>
            _context.AddAsync(entity);

        public virtual Task InsertAsync(IEnumerable<T> entities) =>
            _context.AddRangeAsync(entities);

        #endregion

        #region Sync

        public virtual T GetById(params object[] id) =>
            _context.Find<T>(id);

        public virtual void Insert(T entity) =>
            _context.Add(entity);

        public virtual void Insert(IEnumerable<T> entities) =>
            _context.AddRange(entities);

        public virtual void Update(T entity) =>
            _context.Update(entity);

        public virtual void Update(IEnumerable<T> entities) =>
            _context.UpdateRange(entities);

        public virtual void Delete(T entity) =>
            _context.Remove(entity);

        public virtual void Delete(params object[] id)
        {
            var entity = _context.Find<T>(id);
            _context.Remove(entity);
        }

        public virtual void Delete(IEnumerable<T> entities) =>
            _context.RemoveRange(entities);

        #endregion
    }
}
