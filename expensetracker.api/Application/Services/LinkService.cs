using expensetracker.api.Application.DTO;
using expensetracker.api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace expensetracker.api.Application.Services;

public class LinkService : ILinkService
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    private readonly IActionContextAccessor _actionContextAccessor;

    public LinkService(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
    {
        _urlHelperFactory = urlHelperFactory;
        _actionContextAccessor = actionContextAccessor;
    }

    public List<LinkDto> GenerateLinks<T>(Guid id)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
        var typeName = typeof(T).Name.ToLower().Replace("dto","");
        return new List<LinkDto>
            {
                new LinkDto(urlHelper.Link($"Get{typeName}", new { id }), "self", "GET"),
                new LinkDto(urlHelper.Link($"Put{typeName}", new { id }), "update", "PUT"),
                new LinkDto(urlHelper.Link($"Delete{typeName}", new { id }), "delete", "DELETE"),
                new LinkDto(urlHelper.Link($"Post{typeName}",""), "post", "")
            };
    }

    public List<LinkDto> GeneratePaginationLinks<T>(int pageNumber, int pageSize, int totalCount)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
        var typeName = typeof(T).Name.ToLower().Replace("dto", "");
        var links = new List<LinkDto>
            {
                new LinkDto(urlHelper.Link($"Get{typeName}s", new { pageNumber, pageSize }), "self", "GET")
            };
        if (pageNumber > 1)
        {
            links.Add(new LinkDto(urlHelper.Link($"Get{typeName}s", new { pageNumber = pageNumber - 1, pageSize }), "previousPage", "GET"));
        }
        if (pageNumber * pageSize < totalCount)
        {
            links.Add(new LinkDto(urlHelper.Link($"Get{typeName}s", new { pageNumber = pageNumber + 1, pageSize }), "nextPage", "GET"));
        }
        return links;
    }
}

