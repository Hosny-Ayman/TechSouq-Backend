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
    public class PaymentWayRepository : IPaymentWayRepository
    {

        private readonly AppDbContext _context;

        public PaymentWayRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(PaymentWay paymentWay)
        {
            await _context.PaymentWays.AddAsync(paymentWay);

            var save = await _context.SaveChangesAsync();

            return save>0? paymentWay.Id:0;
        }

        public async Task<bool> Delete(int id)
        {
            var paymentWay = await _context.PaymentWays
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (paymentWay is null)
                return false;

            _context.PaymentWays.Remove(paymentWay);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PaymentWay?> GetById(int id)
        {
            return await _context.PaymentWays
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaymentWay?> GetByName(string name)
        {
            return await _context.PaymentWays
               .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<bool> IsNameExists(string name)
        {
            return await _context.PaymentWays
              .AnyAsync(x => x.Name == name);
        }

        public async Task<bool> Update(PaymentWay paymentWay)
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
