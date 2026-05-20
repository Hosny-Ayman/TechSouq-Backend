using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface ICartItemRepository
    {

        Task<int> AddCartItem(CartItem cartItem);

        Task<List<CartItem>> GetCartItems(int id, bool trackingChanges = true);

        Task<bool> UpdateCartItems (int userId, List<CartItem> cartItems);

        Task<bool> RemoveCartItem(int cartId, int productId);

        Task<bool> IsCartItemExists(int CartItemId);

        Task<int> AddCartAndCartItems(CartItem cartItem , Cart Cart);

        Task<bool> UpdateCartItem(int CartId, int ProductId);

        Task<CartItem> GetCartItemAsync(int CartId, int ProductId,bool trackingChanges = true);

        Task<int> GetCartItemsLengthAsync(int CartId);

        Task<int> AddCartItems(List<CartItem> cartItems,Cart cart);

        Task<decimal> GetCartItemsTotalAmounts(decimal ShippingCost,int cartId);

       





    }
}
