using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Queries
{
    public interface IOrderQueryService
    {

        Task<decimal> ConfirmOrderAsync(ConfirmOrderDto confirmOrderDto, int cartId, int userId);
        Task<List<OrderSummarieDto>> GetOrderSummaryAsync(int cartId, int userId);


    }
}
