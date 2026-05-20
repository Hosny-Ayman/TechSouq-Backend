using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface ISystemSettingsRepository
    {

        Task<int> Add(SystemSettings systemSettings);

        Task<bool> Update(SystemSettings systemSettings);

        Task<bool> Delete(int id);

        Task<SystemSettings?> GetById(int id);

        Task<SystemSettings?> GetByKey(string Key);

        Task<bool> IsExists(string settingKey);
    }
}
