using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class CartItemsWithProductDetailsDto
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; } 
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public int Stock { get; set; }
        public decimal? PriceAfterDiscount { get; set; }
        public bool IsFreeShipping { get; set; }
    }
}
