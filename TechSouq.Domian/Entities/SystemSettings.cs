using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Domain.Entities
{
    public class SystemSettings
    {

       public int Id { get; set; }
       public string SettingKey { get; set; }
       public string SettingValue { get; set; }
       public string? Description { get; set; }
    }
}
