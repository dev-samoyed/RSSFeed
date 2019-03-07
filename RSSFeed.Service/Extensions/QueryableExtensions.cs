using RSSFeed.Service.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace RSSFeed.Service.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> OrderWithDirectionBy<TSource, TKey>(this IQueryable<TSource> source, bool isFirst, SortDirectionType direction, Expression<Func<TSource, TKey>> selector)
        {
            return !isFirst
                ? direction == SortDirectionType.Ascending
                    ? ((IOrderedQueryable<TSource>)source).ThenBy(selector)
                    : ((IOrderedQueryable<TSource>)source).ThenByDescending(selector)
                : direction == SortDirectionType.Ascending
                    ? source.OrderBy(selector)
                    : source.OrderByDescending(selector);
        }
    }
}
