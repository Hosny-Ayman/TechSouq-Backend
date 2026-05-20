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
    public class DeliveryZoneRepository : IDeliveryZone
    {
        private readonly AppDbContext _appDbContext;
        public DeliveryZoneRepository(AppDbContext appDbContext) {

            _appDbContext = appDbContext;
        }

        public async Task<int> AddDeliveryZone(DeliveryZone DeliveryZone)
        {
            _appDbContext.DeliveryZones.Add(DeliveryZone);

            var save = await _appDbContext.SaveChangesAsync();

            return save > 0 ? DeliveryZone.Id : 0;
        }

        public async Task<bool> DeleteDeliveryZone(int DeliveryZoneId)
        {
            return await _appDbContext.DeliveryZones.Where(x => x.Id == DeliveryZoneId).ExecuteDeleteAsync() > 0;
        }

        public async Task<ICollection<DeliveryZone>> GetAllDeliveryZones()
        {
            return await _appDbContext.DeliveryZones.AsNoTracking().ToListAsync();
        }

        public async Task<decimal> GetOnlyShippingCost(string ShippingCity)
        {
            return await _appDbContext.DeliveryZones.AsNoTracking().Where(x=>x.Name == ShippingCity).Select(x=>x.ShippingCost).FirstOrDefaultAsync();
        }

        public Task<bool> UpdateDeliveryZone(DeliveryZone DeliveryZone)
        {
            throw new NotImplementedException();
        }
    }
}
