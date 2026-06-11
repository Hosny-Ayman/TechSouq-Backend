using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class BestSellingProductsDto
    {

        public string ProductName {  get; set; }
        public int TotalSell { get; set; }
        public string CategoryName { get; set; }
        public double PercentageOfSell { get; set; }

    }
}
