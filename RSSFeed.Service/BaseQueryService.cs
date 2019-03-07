using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RSSFeed.Data.Interfaces;
using RSSFeed.Service.Query;
using RSSFeed.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RSSFeed.Service
{
    public abstract class BaseQueryService<TEntity, TModel, TSortType> : BaseService, IBaseQueryService<TEntity, TModel, TSortType>
        where TEntity : class, IEntityBase, new()
    {
        public BaseQueryService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        { }

        #region Abstract

        protected abstract IQueryable<TEntity> Order(IQueryable<TEntity> items, bool isFirst, QueryOrder<TSortType> order);
        protected abstract IQueryable<TEntity> Search(IQueryable<TEntity> items, QuerySearch search);
        #endregion

        public virtual Task<QueryResponse<TModel>> GetAsync(QueryRequest<TSortType> query) => GetAsync(query, _uow.GetRepository<TEntity>().All());

        protected virtual async Task<QueryResponse<TModel>> GetAsync(QueryRequest<TSortType> query, IQueryable<TEntity> items)
        {
            var result = new QueryResponse<TModel>();
            // unclude properties
            items = Include(items, query.Includes);
            // search filter
            items = Search(items, query.Search);
            // get totla count
            result.RecordsTotal = items.Count();
            // order items
            items = Order(items, query.OrderQueries);
            // paging
            items = Paging(items, query.Start, query.Length);
            // get filtered items list
            result.Data = _mapper.Map<IList<TModel>>(await items.ToListAsync());
            // get filtered count
            result.RecordsFiltered = result.Data.Count();
            // return query response
            return result;
        }

        protected virtual IQueryable<TEntity> Include(IQueryable<TEntity> items, IEnumerable<string> includes)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    items = items.Include(include);
                }
            }
            return items;
        }

        public static Expression<Func<T, bool>> SearchAllFields<T>(string searchText)
        {
            var t = Expression.Parameter(typeof(T));
            Expression body = Expression.Constant(false);

            var containsMethod = typeof(string).GetMethod("Contains"
                , new[] { typeof(string) });
            var toStringMethod = typeof(object).GetMethod("ToString");

            var stringProperties = typeof(T).GetProperties()
                .Where(property => property.PropertyType == typeof(string));

            foreach (var property in stringProperties)
            {
                var stringValue = Expression.Call(Expression.Property(t, property.Name),
                    toStringMethod);
                var nextExpression = Expression.Call(stringValue,
                    containsMethod,
                    Expression.Constant(searchText));

                body = Expression.OrElse(body, nextExpression);
            }

            return Expression.Lambda<Func<T, bool>>(body, t);
        }

        protected virtual IQueryable<TEntity> Order(IQueryable<TEntity> items, IEnumerable<QueryOrder<TSortType>> order)
        {
            if (order != null)
            {
                // sort order types
                var isFirst = true;
                foreach (var queryOrder in order.OrderBy(i => i.Order))
                {
                    // for all order types
                    items = Order(items, isFirst, queryOrder);
                    // for next iterations
                    isFirst = false;
                }
            }
            // retuen ordered items
            return items;
        }

        protected virtual IQueryable<TEntity> Paging(IQueryable<TEntity> items, int? start, int? length)
        {
            if (start > 0) items = items.Skip(start.Value);
            if (length > 0) items = items.Take(length.Value);
            return items;
        }
    }
}
