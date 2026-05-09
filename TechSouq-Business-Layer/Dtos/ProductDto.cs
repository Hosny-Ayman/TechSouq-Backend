using Microsoft.Extensions.Hosting;

namespace TechSouq.Application.Dtos
{
    public class ProductDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

        public List<string> Images { get; set; }
        public string FirstImage { get; set; }

        public string CategoryName { get; set; }
        

        public string BrandName { get; set; }

        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public bool IsFreeShipping { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

    }

}
