using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechSouq.Application.Dtos
{
    public class AdminOrderListDto
    {

        public int Id {  get; set; }

        public string CustomerName { get; set; }

        public DateTime Date { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
