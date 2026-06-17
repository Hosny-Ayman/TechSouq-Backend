using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IDeliveryZone
    {
        Task<int> AddDeliveryZone(DeliveryZone DeliveryZone);

        Task<ICollection<DeliveryZone>> GetAllDeliveryZones();

        Task<bool> UpdateDeliveryZone(DeliveryZone DeliveryZone);

        Task<bool> DeleteDeliveryZone(int DeliveryZoneId);

        Task<decimal> GetOnlyShippingCost(string ShippingCity);

        Task<DeliveryZone?> GetById(int id);

    }
}
