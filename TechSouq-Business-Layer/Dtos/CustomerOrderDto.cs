using TechSouq.Domain.Enums;

namespace TechSouq.Application.Dtos
{
    public class CustomerOrderDto
    {
        public int Id { get; set; } 
        public string OrderNumber { get; set; } 
        public DateTime OrderDate { get; set; } 
        public decimal TotalPrice { get; set; } 
        public OrderStatus OrderStatus { get; set; } 
        public int TotalItems { get; set; } 
    }
}
