using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task <int> AddOrder(Order order);

        Task<Order> GetOrder(int orderId, bool trackingChanges = true);

        Task<bool> UpdateOrder(Order order);

        Task<bool> DeleteOrder(int orderId);

        Task<bool> IsOrderExists(int OrderId);

        Task<bool> IsOrderExists(int OrderId, int UserId);


    }
}
