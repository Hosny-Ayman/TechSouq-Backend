using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<int> AddRole(Role role);
        Task<Role> GetRole(int roleId, bool trackingChanges = true);
        Task<bool> UpdateRole(Role role);
        Task<bool> DeleteRole(int roleId);
        Task<bool> IsRoleExists(int roleId);
    }
}