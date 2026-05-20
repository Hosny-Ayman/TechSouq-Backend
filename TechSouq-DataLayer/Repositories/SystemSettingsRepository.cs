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
    public class SystemSettingsRepository: ISystemSettingsRepository
    {
        private readonly AppDbContext _context;

        public SystemSettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(SystemSettings systemSettings)
        {
            await _context.SystemSettings.AddAsync(systemSettings);

           var save = await _context.SaveChangesAsync();

            return save > 0 ? systemSettings.Id : 0;
        }

        public async Task<bool> Delete(int id)
        {
            var setting = await _context.SystemSettings
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (setting is null)
                return false;

            _context.SystemSettings.Remove(setting);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<SystemSettings?> GetById(int id)
        {
            return await _context.SystemSettings.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SystemSettings?> GetByKey(string Key)
        {
            return await _context.SystemSettings.AsNoTracking()
               .FirstOrDefaultAsync(x => x.SettingKey == Key);
        }

        public async Task<bool> IsExists(string settingKey)
        {
            return await _context.SystemSettings
                .AnyAsync(x => x.SettingKey == settingKey);
        }

        public async Task<bool> Update(SystemSettings systemSettings)
        {
            _context.SystemSettings.Update(systemSettings);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
