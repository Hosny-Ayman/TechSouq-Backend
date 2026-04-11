using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User> GetUser(int userId, bool trackingChanges = true);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
        Task<bool> IsUserExists(int userId);
        Task<User> GetUserByEmailAsync(string Email, bool trackingChanges = false);
    }
}