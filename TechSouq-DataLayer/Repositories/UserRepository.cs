using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;

        public async Task<int> AddUser(User user)
        {
            _appDbContext.Users.Add(user);
            var save = await _appDbContext.SaveChangesAsync();
            return save > 0 ? user.Id : 0;
        }

        public async Task<bool> DeleteUser(int userId) => await _appDbContext.Users.Where(x => x.Id == userId).ExecuteDeleteAsync() > 0;

        public async Task<User> GetUser(int userId, bool trackingChanges = true)
        {
            var query = _appDbContext.Users.AsQueryable();
            if (!trackingChanges) query = query.AsNoTracking();
            return await query.Include(x=>x.Role).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<bool> UpdateUser(User user) => await _appDbContext.SaveChangesAsync() > 0;

        public async Task<bool> IsUserExists(int userId) => await _appDbContext.Users.AnyAsync(x => x.Id == userId);

        public async Task<User> GetUserByEmailAsync(string Email, bool trackingChanges = false)
        {
            var query = _appDbContext.Users.AsQueryable();

            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.Include(u=>u.Role).FirstOrDefaultAsync(x => x.Email == Email);
        }
    }
}