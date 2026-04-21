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

        Task<bool> UpdateCartItems (int userId,CartItem cartItem);

        Task<bool> RemoveCartItem(int cartId, int productId);

        Task<bool> IsCartItemExists(int CartItemId);

        Task<bool> AddCartAndCartItems(CartItem cartItem , Cart Cart);

        Task<bool> AddOrUpdateCartItemAsync(int CartId, int ProductId);

    }
}
