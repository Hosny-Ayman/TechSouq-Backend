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
             _Context.Addresses.Add(address);

            var save = await _Context.SaveChangesAsync();


            return save > 0 ? address.Id : 0;

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

        public async Task<Address> GetAddressById(int AddresId,bool trackingChanges=true)
        {
            var query = _Context.Addresses.AsQueryable();
            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == AddresId);
        }

        public async Task<bool> IsAddressExists(int AddressId, int userId)
        {

           
            return await _Context.Addresses.AnyAsync(x => x.UserId == userId && x.Id == AddressId);
        }
    }
}
