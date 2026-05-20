using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

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

        public async Task <bool> DeleteAddress(int AddresId , int userId )
        {

            using var transaction = await _Context.Database.BeginTransactionAsync();

            try
            {
                var address = await _Context.Addresses
                    .FirstOrDefaultAsync(x => x.Id == AddresId && x.UserId == userId);

                if (address is null)
                    return false;

                bool wasDefault = address.Active;

                _Context.Addresses.Remove(address);

                await _Context.SaveChangesAsync();

                if (wasDefault)
                {
                    var anotherAddress = await _Context.Addresses
                        .FirstOrDefaultAsync(x => x.UserId == userId);

                    if (anotherAddress != null)
                    {
                        anotherAddress.Active = true;

                        await _Context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
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

        public async Task<Address> GetOnlyDefaultAddress(int userId)
        {
            return await _Context.Addresses.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.Active);
        }

        public async Task<decimal> GetCityShippingCost(int userId,string? CityName)
        {
            if(CityName!=null)
            {
                return await _Context.DeliveryZones.AsNoTracking().Where(x => x.Name == CityName).Select(x => x.ShippingCost).FirstOrDefaultAsync();

            }
            else
            {

                var add = await _Context.Addresses.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.Active);

                if (add != null)
                {
                    return await _Context.DeliveryZones.AsNoTracking().Where(x => x.Name == add.City).Select(x => x.ShippingCost).FirstOrDefaultAsync();
                }

            }

            return 0;
           
        }
    }
}
