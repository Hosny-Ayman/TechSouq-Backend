using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }


    }
}
