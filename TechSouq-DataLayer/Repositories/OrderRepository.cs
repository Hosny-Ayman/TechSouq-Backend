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
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;

        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> AddOrder(Order order)
        {
            _appDbContext.Orders.Add(order);

            var save = await _appDbContext.SaveChangesAsync();

            return save > 0 ? order.Id : 0;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            return await _appDbContext.Orders.Where(x=>x.Id == orderId).ExecuteDeleteAsync()>0;
        }

        public async Task<Order> GetOrder(int orderId, bool trackingChanges = true)
        {
            var query = _appDbContext.Orders.AsQueryable();

            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == orderId );
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            

            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsOrderExists(int OrderId)
        {
            return await _appDbContext.Orders.AnyAsync(x => x.Id == OrderId);
        }

        public async Task<bool> IsOrderExists(int OrderId, int UserId)
        {

            return await _appDbContext.Orders.AnyAsync(x => x.Id == OrderId && x.UserId == UserId);

        }
    }
}
