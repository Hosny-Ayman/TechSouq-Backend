using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Infrastructure.Data;
using TechSouq.Domain.Interfaces;
using TechSouq.Domain.Entities;
using System.Reflection.Metadata.Ecma335;

namespace TechSouq.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _Context;

        public AddressRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task <int>  AddAddress(Address address)
        {
            using var transaction = await _Context.Database.BeginTransactionAsync();

            try
            {
                _Context.Addresses.Add(address);

                var save = await _Context.SaveChangesAsync();

                if (save > 0)
                {
                    var isSaved = await setAsDefaultAsync(address.Id, address.UserId);

                    if (!isSaved)
                    {
                        await transaction.RollbackAsync();
                        return 0;
                    }
                }

                await transaction.CommitAsync();
                return save;
            }
            catch
            {
                await transaction.RollbackAsync();
                return 0;
            }

        }

        public async Task <bool> DeleteAddress(int AddresId)
        {
           

            

            int Rowseffected = await _Context.Addresses.Where(x => x.Id == AddresId).ExecuteDeleteAsync();

            return Rowseffected > 0;
        }

        public async Task <ICollection<Address>> GetAddresses(int UserId)
        {
           
            var address = await _Context.Addresses.AsNoTracking().Where(x=>x.UserId == UserId).ToListAsync();

            return address;

        }

        public async Task <bool> UpdateAddress(Address address)
        {
          

            return await _Context.SaveChangesAsync() > 0;

        }

        public async Task<Address> GetAddressById(int AddresId,int userId,bool trackingChanges=true)
        {
            var query = _Context.Addresses.AsQueryable();
            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == AddresId && x.UserId == userId);
        }

        public async Task<bool> IsAddressExists(int AddressId, int userId)
        {

           
            return await _Context.Addresses.AnyAsync(x => x.UserId == userId && x.Id == AddressId);
        }

        public async Task<bool> setAsDefaultAsync(int AddressId, int userId)
        {


           var effrow = await _Context.Addresses.Where(x=>x.UserId==userId).ExecuteUpdateAsync(s=>s.SetProperty(a=>a.Active,a=>a.Id == AddressId));

            return effrow>0;

        }

        public async Task<int> HowManyAddressesHeHaveAsync(int userId)
        {
            return await _Context.Addresses.CountAsync(x => x.UserId == userId);
        }
    }
}
