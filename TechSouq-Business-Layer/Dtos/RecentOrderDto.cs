namespace TechSouq.Application.Dtos
{
    public class RecentOrderDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal price { get; set; }
        public string Status { get; set; }
        public string ProductImage { get; set; }
    }
}
