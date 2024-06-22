using expensetracker.api.Application.DTO;
namespace expensetracker.api.Application.Services.Links;
public interface ILinkService
{
    List<LinkDto> GenerateLinks<T>(Guid id);
    List<LinkDto> GeneratePaginationLinks<T>(int pageNumber, int pageSize, int totalCount);
}
