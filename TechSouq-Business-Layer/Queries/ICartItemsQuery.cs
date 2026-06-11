using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Queries
{
    public interface ICartItemsQuery
    {

        Task<List<CartItemsWithProductDetailsDto>> GetAllCartItemsWithProductDetailsAsync(int UserId, bool trackingChanges = true);

    }
}
