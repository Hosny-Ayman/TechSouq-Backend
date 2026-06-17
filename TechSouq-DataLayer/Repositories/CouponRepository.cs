using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class CouponRepository : ICouponRepository
    {

        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);

            var save = await _context.SaveChangesAsync();

            return save > 0 ? coupon.Id : 0;
        }

        public async Task<bool> Delete(int id)
        {
            var coupon = await _context.Coupons
              .FirstOrDefaultAsync(x => x.Id == id);

            if (coupon is null)
                return false;

            _context.Coupons.Remove(coupon);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Coupon?> GetById(int id)
        {
            return await _context.Coupons
              .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Coupon> GetOnlyActiveCouponByCode(string code)
        {
            var Discount = await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);

            if(Discount != null && Discount.ExpiryDate > DateTime.UtcNow && Discount.IsActive && Discount.UsageLimit >=1)
            {
                return Discount;
            }
            else
            {
                if(Discount != null)
                {
                    Discount.IsActive = false;
                    _context.Coupons.Update(Discount);

                    await _context.SaveChangesAsync();
                }
                return null;
            }
               
        }

        public async Task<bool> IsCodeExists(string code)
        {
            return await _context.Coupons
                .AnyAsync(x => x.Code == code);
        }

        public async Task RemoveAllExpiredCouponsAsync()
        {
             await _context.Coupons.Where(x => x.ExpiryDate <= DateTime.Now).ExecuteUpdateAsync(s => s.SetProperty(d => d.IsActive, d => false));
        }

        public async Task<bool> Update(Coupon coupon)
        {
          

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
