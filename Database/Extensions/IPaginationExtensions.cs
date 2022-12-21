namespace Kern.Database.Extensions;

public static class IPaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, long? lastId) where T : IPagination
    {
        return lastId != null ?
            query.Where(e => e.Id < lastId).Take(25) :
            query.Take(25);
    }
}