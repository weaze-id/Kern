namespace Kern.Database.Extensions;

public static class ISearchNameExtensions
{
    public static IQueryable<T> SearchName<T>(this IQueryable<T> query, string? searchQuery) where T : ISearchName
    {
        return searchQuery != null ? query.Where(e => e.Name!.ToLower().Contains(searchQuery.ToLower())) : query;
    }
}