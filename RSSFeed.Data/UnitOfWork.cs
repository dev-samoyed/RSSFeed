using Microsoft.EntityFrameworkCore;
using RSSFeed.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSSFeed.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IDictionary<string, IRepository> _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _repositories = new SortedDictionary<string, IRepository>();
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntityBase, new()
        {
            var typeName = typeof(T).FullName;
            if (!_repositories.ContainsKey(typeName))
            {
                _repositories.Add(typeName, new Repository<T>(_context));
            }
            return (IRepository<T>)_repositories[typeName];
        }

        public void SaveChanges() => _context.SaveChanges();
        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        public void RunInTransaction(Action action)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                action.Invoke();
                // commit transaction
                transaction.Commit();
            }
        }

        public async Task RunInTransactionAsync(Func<Task> actionAsync)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                await actionAsync.Invoke();
                // commit transaction
                transaction.Commit();
            }
        }
    }
}
