using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class DashboardDto
    {

        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int OutOfStockProducts { get; set; }


    }
}
