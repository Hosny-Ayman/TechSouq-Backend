using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;

namespace TechSouq.Application.Queries
{
    public interface ICouponsQuery
    {
        Task<PagedResponse<CouponSummaryDto>> GetAllCouponsPaged(int pageNumber, int pageSize, string? CodeSearch);
        Task<CouponDto> GetCouponById(int id);

    }
}
