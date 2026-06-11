using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class ConfirmOrderDto
    {

        public string? Code { get; set; }
        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }
        public string Email { get; set; }
        public string Building { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ShippingFullName { get; set; }
        public int PaymentWayId { get; set; }
        public string? PaymentIntentId { get; set; }


    }
}
