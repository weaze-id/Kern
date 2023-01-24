namespace Kern.Database.Extensions;

public static class IPaginationExtensions
{
    public static int PageSize { get; set; } = 25;

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, long? lastId) where T : IPagination
    {
        return lastId != null ? query.Where(e => e.Id < lastId).Take(PageSize) : query.Take(PageSize);
    }
}