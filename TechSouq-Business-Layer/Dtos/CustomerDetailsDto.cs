using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class CustomerDetailsDto
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<AddressOrderDetilsDto> Addresses { get; set; }
        public List<CustomerOrderDto> RecentOrders { get; set; } 

    }
}
