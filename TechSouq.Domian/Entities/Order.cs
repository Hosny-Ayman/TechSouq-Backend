using TechSouq.Domain.Enums;

namespace TechSouq.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? PaymentIntentId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        public decimal DeliveryCost { get; set; }

        public decimal TotalAmount { get; set; }

       
        public int UserId { get; set; }
        public User User { get; set; }

        
        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }

        
        public string ShippingFullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Country { get; set; }

        public string ShippingCity { get; set; }

        public string ShippingStreet { get; set; }

        public string Building { get; set; }

        
        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

        public int PaymentWayId { get; set; }
        public PaymentWay PaymentWay { get; set; }
    }
}
