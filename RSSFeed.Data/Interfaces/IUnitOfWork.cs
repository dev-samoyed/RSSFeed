using System;
using System.Threading.Tasks;

namespace RSSFeed.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class, IEntityBase, new();
        void SaveChanges();
        Task SaveChangesAsync();
        void RunInTransaction(Action action);
        Task RunInTransactionAsync(Func<Task> actionAsync);
    }
}
