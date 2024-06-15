using expensetracker.api.Application.DTO;
using expensetracker.api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace expensetracker.api.Application.Services;

public class LinkService : ILinkService
{
    private readonly IUrlHelper _urlHelper;

    public LinkService(IUrlHelper urlHelper)
    {
        _urlHelper = urlHelper;
    }

   

    public List<LinkDto> GenerateLinks<T>(Guid id)
    {
        var typeName = typeof(T).Name.ToLower();
        return new List<LinkDto>
        {
            new LinkDto(_urlHelper.Link($"Get{typeName}", new { id }), "self", "GET"),
            new LinkDto(_urlHelper.Link($"Update{typeName}", new { id }), "update", "PUT"),
            new LinkDto(_urlHelper.Link($"Delete{typeName}", new { id }), "delete", "DELETE")
        };
    }
}