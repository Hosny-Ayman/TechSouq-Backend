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

        public async Task<bool> RemoveCartItem(int cartId, int productId)
        {
            return await _appDbContext.CartItems.Where(x => x.CartId == cartId && x.ProductId == productId).ExecuteDeleteAsync() > 0;
        }

        public async Task<List<CartItem>> GetCartItems(int CartItemId, bool trackingChanges = true)
        {
            var query = _appDbContext.CartItems.AsQueryable();
            if(!trackingChanges)
                query = query.AsNoTracking();

            return await query.Where(x => x.Id == CartItemId).ToListAsync();
        }

        public async Task<bool> UpdateCartItems(int userId,CartItem cartItem)
        {
           


            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCartItemExists(int CartItemId)
        {
            return await _appDbContext.CartItems.AnyAsync(x => x.Id == CartItemId);
        }

        public async Task<bool> AddCartAndCartItems(CartItem cartItem, Cart Cart)
        {

            cartItem.Cart = Cart;

            await _appDbContext.Carts.AddAsync(Cart);

            await _appDbContext.CartItems.AddAsync(cartItem);


            return await _appDbContext.SaveChangesAsync() > 0;

        }

        public async Task<bool> AddOrUpdateCartItemAsync(int CartId, int ProductId)
        {

            var IscartitemExists = await _appDbContext.CartItems.FirstOrDefaultAsync(x => x.CartId == CartId && x.ProductId == ProductId);

            if(IscartitemExists != null)
            {
                IscartitemExists.Quantity += 1;
            }
            else
            {

                CartItem cartItem = new CartItem();
                cartItem.CartId = CartId;
                cartItem.ProductId = ProductId;

                _appDbContext.CartItems.Add(cartItem);
            }


            return await _appDbContext.SaveChangesAsync() > 0;

        }
    }
}
