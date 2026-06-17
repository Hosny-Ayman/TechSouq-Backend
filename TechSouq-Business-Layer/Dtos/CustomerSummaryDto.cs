using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class CustomerSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TotalOrders { get; set; } 
        public decimal TotalAmountSpent { get; set; } 
        public bool IsActive { get; set; }


    }
}
