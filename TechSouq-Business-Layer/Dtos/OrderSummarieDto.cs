using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Enums;

namespace TechSouq.Application.Dtos
{
    public class OrderSummarieDto
    {

        public int Id {  get; set; }
        public DateTime Date {  get; set; }
        public OrderStatus Status{  get; set; }
        public string DeliveryAddress{  get; set; }
        public int PaymentWayId {  get; set; }
        public decimal Subtotal{  get; set; }
        public decimal DeliveryCost {  get; set; }
        public decimal DiscountAmount{  get; set; }
        public decimal TotalAmount {  get; set; }
        public List<ItemDto> Items { get; set; }

    }
    public class ItemDto
    {

        public int Id {  get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price {get; set; }
        public int Quantity { get; set; }

    }
}
