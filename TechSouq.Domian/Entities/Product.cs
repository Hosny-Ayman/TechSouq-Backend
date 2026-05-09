namespace TechSouq.Domain.Entities
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }

        public bool IsFreeShipping { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }


        public int CategoryId { get; set; }
        public Categorie Categorie { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }


        public ICollection<ProductImage> ProductImages { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public ICollection<ProductReview> ProductReview { get; set; }
    }
  
}
