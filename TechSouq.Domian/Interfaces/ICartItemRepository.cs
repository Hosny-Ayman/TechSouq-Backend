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

        Task<bool> UpdateCartItems (List <CartItem> cartItem);

        Task<bool> DeleteCartItem (int id);

        Task<bool> IsCartItemExists(int CartItemId);

    }
}
