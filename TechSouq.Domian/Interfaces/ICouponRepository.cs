using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface ICouponRepository
    {
        Task<int> Add(Coupon coupon);

        Task<bool> Update(Coupon coupon);

        Task<bool> Delete(int id);

        Task<Coupon?> GetById(int id);

        Task<bool> IsCodeExists(string code);

        Task<Coupon> GetOnlyActiveCouponByCode(string code);

        Task RemoveAllExpiredCouponsAsync();





    }
}
