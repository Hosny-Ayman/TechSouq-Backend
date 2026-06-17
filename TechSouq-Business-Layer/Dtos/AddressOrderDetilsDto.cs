using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class AddressOrderDetilsDto
    {

        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string? Building { get; set; }
        public string Country { get; set; }
        public bool Active { get; set; }

    }
}
