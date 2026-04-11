using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {

        private readonly AppDbContext _appDbContext;

        public CartItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> AddCartItem(CartItem cartItem)
        {
            _appDbContext.CartItems.Add(cartItem);

           
               var save =  await _appDbContext.SaveChangesAsync();

            return save > 0 ? cartItem.Id : 0;
        }

        public async Task<bool> DeleteCartItem(int CartItemId)
        {
            return await _appDbContext.CartItems.Where(x => x.Id == CartItemId).ExecuteDeleteAsync() > 0;
        }

        public async Task<List<CartItem>> GetCartItems(int CartItemId, bool trackingChanges = true)
        {
            var query = _appDbContext.CartItems.AsQueryable();
            if(!trackingChanges)
                query = query.AsNoTracking();

            return await query.Where(x => x.Id == CartItemId).ToListAsync();
        }

        public async Task<bool> UpdateCartItems(List<CartItem> cartItem)
        {
           

            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCartItemExists(int CartItemId)
        {
            return await _appDbContext.CartItems.AnyAsync(x => x.Id == CartItemId);
        }
    }
}
