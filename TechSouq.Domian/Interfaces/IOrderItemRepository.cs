using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<int> AddOrderItem(OrderItem orderItem);

        Task<OrderItem> GetOrderItem(int orderItem, bool trackingChanges = true);

        Task<bool> UpdateOrderItem(OrderItem orderItem );

        Task<bool> DeleteOrderItem(int orderItemId);

        Task<bool> IsOrderItemExists(int OrderItemId);

    }
}
