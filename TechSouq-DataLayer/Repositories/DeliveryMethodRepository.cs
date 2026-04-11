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
    public class DeliveryMethodRepository : IDeliveryMethodRepository
    {
        private readonly AppDbContext _appDbContext;

        public DeliveryMethodRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }


        public async Task<int> AddDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            _appDbContext.DeliveryMethods.Add(deliveryMethod);

            var save  = await _appDbContext.SaveChangesAsync();

            return save>0? deliveryMethod.Id:0;
        }

        public async Task<bool> DeleteDeliveryMethod(int deliveryMethodId)
        {
            return await _appDbContext.DeliveryMethods.Where(x=>x.Id == deliveryMethodId).ExecuteDeleteAsync() > 0;
        }

        public async Task<DeliveryMethod> GetDeliveryMethod(int deliveryMethodId, bool trackingChanges = true)
        {
            var query = _appDbContext.DeliveryMethods.AsQueryable();

            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == deliveryMethodId);
        }

        public async Task<bool> UpdateDeliveryMethod(DeliveryMethod deliveryMethod)
        {
          
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsDeliveryMethodExists(int DeliveryMethodId)
        {
            return await _appDbContext.DeliveryMethods.AnyAsync(x => x.Id == DeliveryMethodId);
        }
    }
}
