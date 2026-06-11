using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Enums;

namespace TechSouq.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<int> AddCart(Cart Cart);

        Task<Cart> GetCart(int CartId, bool trackingChanges = true);

        Task<bool> UpdateCart (Cart Cart);

        Task <bool> DeleteCart(int CartId);

        Task<bool> IsCartExists(int CartId);

        Task<Cart> GetCartIdbyUserId(int userId);

        Task<bool> ChangeCartStatus(int CartId, SystemEnums cartStatus);

        Task<Cart> GetCartIdbyUserIdAnyStatus(int userId);
    }
}
