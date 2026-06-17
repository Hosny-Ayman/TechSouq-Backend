using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;

namespace TechSouq.Application.Queries
{
    public interface IDeliveryZonesQuery
    {
        Task<PagedResponse<DeliveryZoneDto>> GetAllDeliveryZonesPaged(int pageNumber, int pageSize, string? NameSearch);

    }
}
