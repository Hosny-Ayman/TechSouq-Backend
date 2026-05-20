using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Enums;

namespace TechSouq.Domain.Entities
{
    public class PaymentWay
    {

        public int Id { get; set; }
        public string Name  { get; set; }


        public ICollection<Order> Orders { get; set; }


    }
}
