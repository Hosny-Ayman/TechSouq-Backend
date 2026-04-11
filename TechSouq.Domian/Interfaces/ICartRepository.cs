using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<int> AddCart(Cart Cart);

        Task<Cart> GetCart(int CartId, bool trackingChanges = true);

        Task<bool> UpdateCart (Cart Cart);

        Task <bool> DeleteCart(int CartId);

        Task<bool> IsCartExists(int CartId);
    }
}
