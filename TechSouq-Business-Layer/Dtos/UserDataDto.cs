using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class LoginResponseDto
    {

        public UserDto User { get; set; }
        public TokenDto Token { get; set; }

    }
}
