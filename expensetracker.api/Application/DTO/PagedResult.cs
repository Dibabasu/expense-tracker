using Microsoft.EntityFrameworkCore;

namespace expensetracker.api.Application.DTO;


public class PagedResult<T>
{
    public IList<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public List<LinkDto>? Links { get; set; }

    public PagedResult(IList<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        PageNumber = pageNumber;
    }

    public static async Task<PagedResult<T>> GetPagedResultAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var totalCount = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<T>(items, totalCount, pageSize, pageNumber);
    }
}
