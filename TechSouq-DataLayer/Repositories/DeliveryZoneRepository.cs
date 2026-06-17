using AutoMapper;
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
        private readonly IMapper _mapper;
        public DeliveryZoneRepository(AppDbContext appDbContext, IMapper mapper) {

            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<int> AddDeliveryZone(DeliveryZone DeliveryZone)
        {
            _appDbContext.DeliveryZones.Add(DeliveryZone);

            var save = await _appDbContext.SaveChangesAsync();

            return save > 0 ? DeliveryZone.Id : 0;
        }

        public async Task<DeliveryZone?> GetById(int id)
        {
            return await _appDbContext.DeliveryZones.AsNoTracking()
              .FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<bool> UpdateDeliveryZone(DeliveryZone DeliveryZone)
        {
            var existingDeliveryZone = await _appDbContext.DeliveryZones.FindAsync(DeliveryZone.Id);

            if(existingDeliveryZone is null)
                return false;


            existingDeliveryZone.Name = DeliveryZone.Name;
            existingDeliveryZone.ShippingCost = DeliveryZone.ShippingCost;

             await _appDbContext.SaveChangesAsync();

            return true;


        }
    }
}
