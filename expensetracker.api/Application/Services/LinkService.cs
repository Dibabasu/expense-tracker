using System.Text.RegularExpressions;
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
        var typeName = GetTypeName<T>();

        var links = new List<LinkDto>
        {
            new LinkDto(urlHelper.Link($"Get{typeName}", new { id }), "self", "GET"),
            new LinkDto(urlHelper.Link($"Update{typeName}", new { id }), "update", "PUT"),
            new LinkDto(urlHelper.Link($"Delete{typeName}", new { id }), "delete", "DELETE")
        };

        // Add a separate entry for POST without id
        var postLink = urlHelper.Link($"Create{typeName}", new { });
        if (postLink != null)
        {
            links.Add(new LinkDto(postLink, "post", "POST"));
        }

        return links;
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
    private static string GetTypeName<T>()
    {
        var typeName = typeof(T).Name;
        return Regex.Replace(typeName, "Dto$", "", RegexOptions.IgnoreCase).ToLower();
    }
}

