using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domian.Interfaces
{
    public interface IBrandRepository
    {

        Task <int> AddBrand (Brand brand);

        Task<Brand> GetBrand(int BrandId, bool trackingChanges = true);

        Task <bool> UpdateBrand(Brand brand);

        Task <bool> DeleteBrand (int brandId);

        Task<bool> IsBrandExists(int CartItemId);



    }
}
