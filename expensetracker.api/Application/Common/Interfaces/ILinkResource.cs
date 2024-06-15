using expensetracker.api.Application.DTO;

namespace expensetracker.api.Application.Common.Interfaces
{
    public interface ILinkResource
    {
        Guid Id { get; }
        List<LinkDto> Links { get; set; }
    }
}