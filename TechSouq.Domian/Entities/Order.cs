namespace TechSouq.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public int AddressId { get; set; }
        public  Address Address { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public int? DeliveryZoneId { get; set; }
        public DeliveryZone DeliveryZone {  get; set; }

        public string? ShippingGovernorate { get; set; }
       
        public decimal DeliveryCost { get; set; }


    }
}
