using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IPaymentWayRepository
    {

        Task<int> Add(PaymentWay paymentWay);

        Task<bool> Update(PaymentWay paymentWay);

        Task<bool> Delete(int id);

        Task<PaymentWay?> GetById(int id);

        Task<PaymentWay?> GetByName(string name);

        Task<bool> IsNameExists(string name);
    }
}
