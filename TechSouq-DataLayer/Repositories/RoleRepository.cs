using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _appDbContext;
        public RoleRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;

        public async Task<int> AddRole(Role role)
        {
            _appDbContext.Roles.Add(role);
            var save = await _appDbContext.SaveChangesAsync();
            return save > 0 ? role.Id : 0;
        }

        public async Task<bool> DeleteRole(int roleId) => await _appDbContext.Roles.Where(x => x.Id == roleId).ExecuteDeleteAsync() > 0;

        public async Task<Role> GetRole(int roleId, bool trackingChanges = true)
        {
            var query = _appDbContext.Roles.AsQueryable();
            if (!trackingChanges) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id == roleId);
        }

        public async Task<bool> UpdateRole(Role role) => await _appDbContext.SaveChangesAsync() > 0;

        public async Task<bool> IsRoleExists(int roleId) => await _appDbContext.Roles.AnyAsync(x => x.Id == roleId);
    }
}