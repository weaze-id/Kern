using System.Linq.Expressions;

namespace Kern.Database.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> DoWhere<T>(this IQueryable<T> query, Expression<Func<T, bool>>? where)
    {
        return where != null ? query.Where(where) : query;
    }
}