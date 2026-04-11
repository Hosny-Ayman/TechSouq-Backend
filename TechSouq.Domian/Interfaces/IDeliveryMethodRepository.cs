using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IDeliveryMethodRepository
    {
        Task<int> AddDeliveryMethod(DeliveryMethod deliveryMethod);

        Task<DeliveryMethod> GetDeliveryMethod(int deliveryMethodId, bool trackingChanges = true);

        Task<bool> UpdateDeliveryMethod(DeliveryMethod deliveryMethod);

        Task<bool> DeleteDeliveryMethod(int deliveryMethodId);

        Task<bool> IsDeliveryMethodExists(int CDeliveryMethodId);
    }
}
