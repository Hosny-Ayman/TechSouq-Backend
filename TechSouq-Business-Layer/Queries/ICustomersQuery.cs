using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;

namespace TechSouq.Application.Queries
{
    public interface ICustomersQuery
    {
        Task<PagedResponse<CustomerSummaryDto>> GetAllCustomersPaged(int pageNumber, int pageSize,string?EmailSearch);

        Task<CustomerDetailsDto> GetCustomerDetails(int CustomerId);

        Task<bool> SetActive(int CustomerId);
    }
}
