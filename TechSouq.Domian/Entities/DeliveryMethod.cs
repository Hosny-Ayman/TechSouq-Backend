using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Domain.Entities
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

       
    }

}
