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
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            var deletedCount = await _appDbContext.CartItems
                .Where(x => x.CartId == cartId && x.ProductId == productId)
                .ExecuteDeleteAsync();

            if (deletedCount == 0)
                return false;

            var hasItems = await _appDbContext.CartItems
                .AnyAsync(x => x.CartId == cartId);

            if (!hasItems)
            {
                await _appDbContext.Carts
                    .Where(x => x.Id == cartId)
                    .ExecuteDeleteAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public async Task<List<CartItem>> GetCartItems(int CartId, bool trackingChanges = true)
        {
            var query = _appDbContext.CartItems.AsQueryable();
            if(!trackingChanges)
                query = query.AsNoTracking();

            return await query.Where(x => x.CartId == CartId).ToListAsync();
        }

        public async Task<bool> UpdateCartItems(int userId, List<CartItem> cartItems)
        {
            var userCart = await _appDbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.Status == Domain.Enums.SystemEnums.Active);

            if (userCart == null) return false;

            foreach (var item in cartItems)
            {
                var existingItem = await _appDbContext.CartItems
                    .FirstOrDefaultAsync(c => c.Id == item.Id && c.CartId == userCart.Id);

                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                }
            }

            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCartItemExists(int CartItemId)
        {
            return await _appDbContext.CartItems.AnyAsync(x => x.Id == CartItemId);
        }

        public async Task<int> AddCartAndCartItems(CartItem cartItem, Cart Cart)
        {

            cartItem.Cart = Cart;

            await _appDbContext.Carts.AddAsync(Cart);

            await _appDbContext.CartItems.AddAsync(cartItem);


             await _appDbContext.SaveChangesAsync();

            return Cart.Id;

        }

        public async Task<bool> UpdateCartItem(int CartId, int ProductId)
        {

            var IscartitemExists = await _appDbContext.CartItems.FirstOrDefaultAsync(x => x.CartId == CartId && x.ProductId == ProductId);
          

            if (IscartitemExists!=null)
            {
                IscartitemExists.Quantity += 1;

                return await _appDbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }

        }

        public async Task<CartItem> GetCartItemAsync(int CartId, int ProductId, bool trackingChanges = true)
        {
            var query = _appDbContext.CartItems.AsQueryable();

            if(!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.CartId == CartId && x.ProductId == ProductId);
        }

        public async Task<int> GetCartItemsLengthAsync(int CartId)
        {

            return await _appDbContext.CartItems.CountAsync(x => x.CartId == CartId);

        }

        public async Task<int> AddCartItems(List<CartItem> cartItems, Cart cart)
        {


            if (cartItems == null || !cartItems.Any()) return 0;

            if (cart.Id == 0)
            {
                await _appDbContext.Carts.AddAsync(cart);
                await _appDbContext.SaveChangesAsync();
            }

            int cartId = cart.Id;


            var existingItemsDict = await _appDbContext.CartItems
                .Where(x => x.CartId == cartId)
                .ToDictionaryAsync(x=>x.ProductId);

             

            foreach (var item in cartItems)
            {
               

                if (existingItemsDict.TryGetValue(item.ProductId,out var existingItem))
                {

                    var productQuantity = await _appDbContext.Products.AsNoTracking().Where(x => x.Id == existingItem.ProductId).Select(x => x.Stock).FirstOrDefaultAsync();           

                    existingItem.Quantity += item.Quantity;

                    if(existingItem.Quantity > productQuantity)
                        existingItem.Quantity = productQuantity;

                }
                else
                {
                    item.CartId = cartId;

                    var productQuantity = await _appDbContext.Products.AsNoTracking().Where(x => x.Id == item.ProductId).Select(x => x.Stock).FirstOrDefaultAsync();

                    if (item.Quantity > productQuantity)
                        item.Quantity = productQuantity;

                    await _appDbContext.CartItems.AddAsync(item);

                    existingItemsDict.Add(item.ProductId, item);
                }
            }

            await _appDbContext.SaveChangesAsync();

            return cartId;

        }

        public async Task<decimal> GetCartItemsTotalAmounts(decimal ShippingCost, int cartId)
        {

            var total = await _appDbContext.CartItems.Where(x => x.CartId == cartId).SumAsync(x => (x.Product.PriceAfterDiscount ?? x.Product.Price) *  x.Quantity);

            return total + ShippingCost;
        }

    }
}
