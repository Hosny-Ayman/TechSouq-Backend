using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface  IAddressRepository
    {

        Task <int> AddAddress(Address address);

        Task <ICollection<Address>> GetAddresses(int UserId);

        Task <bool> UpdateAddress(Address address);

        Task <bool> DeleteAddress(int AddresId);

        Task <Address> GetAddressById(int AddresId,int userId, bool trackingChanges = true);

        Task<bool> IsAddressExists(int AddressId, int userId);

        Task<bool> setAsDefaultAsync(int AddressId, int userId);

        Task<int> HowManyAddressesHeHaveAsync(int userId);


    }
}
