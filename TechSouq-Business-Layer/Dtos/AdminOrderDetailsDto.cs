using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Enums;

namespace TechSouq.Application.Dtos
{
    public class AdminOrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }
        public string DeliveryAddress { get; set; }

        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public int PaymentWayId { get; set; }
        public string StripePaymentIntentId { get; set; } 

        public List<AdminOrderItemDto> Items { get; set; }


    }

    public class AdminOrderItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
