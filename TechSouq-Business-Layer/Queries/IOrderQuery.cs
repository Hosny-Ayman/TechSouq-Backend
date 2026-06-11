using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Domain.Enums;

namespace TechSouq.Application.Queries
{
    public interface IOrderQuery
    {

        Task<decimal> ConfirmOrderAsync(ConfirmOrderDto confirmOrderDto, int cartId, int userId, bool calculateOnly = false);
        Task<List<OrderSummarieDto>> GetOrderSummaryAsync(int cartId, int userId);
        Task<PagedResponse<AdminOrderListDto>> GetAllOrdersPaged(int PageNumber,int PageSize,OrderStatus? status,string search);
        Task<AdminOrderDetailsDto> GetOrderDtailsAdmin(int OrderId);


    }
}
