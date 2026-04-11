using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _appDbContext;

        public OrderItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<int> AddOrderItem(OrderItem orderItem)
        {
            _appDbContext.OrderItems.Add(orderItem);

            var save = await _appDbContext.SaveChangesAsync();

            return save > 0 ? orderItem.Id : 0;
        }

        public async Task<bool> DeleteOrderItem(int orderItemId)
        {
            return await _appDbContext.OrderItems.Where(x => x.Id == orderItemId).ExecuteDeleteAsync() > 0;
        }

        public async Task<OrderItem> GetOrderItem(int orderItemId, bool trackingChanges = true)
        {
            var query = _appDbContext.OrderItems.AsQueryable();
            if(!trackingChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(x => x.Id == orderItemId);


        }

        public async Task<bool> UpdateOrderItem(OrderItem orderItem)
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsOrderItemExists(int OrderItemId)
        {
            return await _appDbContext.OrderItems.AnyAsync(x => x.Id == OrderItemId);
        }
    }
}
