using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Application.DTO;
using expensetracker.api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace expensetracker.api.Controllers;
public abstract class BaseController<T> : ControllerBase where T : class
{
    private readonly ILogger _logger;
    private readonly ILinkService _linkService;

    protected BaseController(ILogger logger, ILinkService linkService)
    {
        _logger = logger;
        _linkService = linkService;
    }

    protected void AddLinks(T resource)
    {
        if (resource is ILinkResource linkResource)
        {
            linkResource.Links = _linkService.GenerateLinks<T>(linkResource.Id);
        }
    }

    protected void AddPaginationLinks(PagedResult<T> pagedResult, int pageNumber, int pageSize)
    {
        var links = _linkService.GeneratePaginationLinks<T>(pageNumber, pageSize, pagedResult.TotalCount);
        pagedResult.Links = links;
    }
}