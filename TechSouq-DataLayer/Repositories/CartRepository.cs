using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Enums;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {

        private readonly AppDbContext _appDbContext;

        public CartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> AddCart(Cart Cart)
        {
           _appDbContext.Carts.Add(Cart);

           var save = await _appDbContext.SaveChangesAsync();

            return save > 0 ? Cart.Id : 0;
        }

        public async Task<bool> DeleteCart(int CartId)
        {
           return await _appDbContext.Carts.Where(x => x.Id == CartId).ExecuteDeleteAsync() > 0;
        }

        public async Task<Cart> GetCart(int CartId, bool trackingChanges = true)
        {
            var query = _appDbContext.Carts.AsQueryable();

            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == CartId);
        }

        public async Task<bool> UpdateCart(Cart Cart)
        {
           

            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCartExists(int CartId)
        {
            return await _appDbContext.Carts.AnyAsync(x => x.Id == CartId);
        }

        public async Task<Cart> GetCartIdbyUserId(int userId)
        {
            var Cart = await _appDbContext.Carts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId&&x.Status==SystemEnums.Active);

            return Cart;

            
        }

        public async Task<Cart> GetCartIdbyUserIdAnyStatus(int userId)
        {
            var Cart = await _appDbContext.Carts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

            return Cart;


        }

        public async Task<bool> ChangeCartStatus(int CartId, SystemEnums cartStatus)
        {
            var cart = await _appDbContext.Carts.FindAsync(CartId);

            if (cart is null)
                return false;

            cart.Status = cartStatus;

            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
