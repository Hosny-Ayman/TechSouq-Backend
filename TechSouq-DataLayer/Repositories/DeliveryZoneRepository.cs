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

        public Task<int> AddDeliveryZone(DeliveryZone DeliveryZone)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDeliveryZone(int DeliveryZoneId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<DeliveryZone>> GetAllDeliveryZones()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDeliveryZone(DeliveryZone DeliveryZone)
        {
            throw new NotImplementedException();
        }
    }
}
