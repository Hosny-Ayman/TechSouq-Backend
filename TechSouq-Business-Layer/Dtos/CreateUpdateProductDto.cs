using Microsoft.AspNetCore.Http;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.Dtos
{
    public class CreateUpdateProductDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }

        public decimal? PriceAfterDiscount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public bool IsFreeShipping { get; set; }

        public IFormFile? MainImage { get; set; } 
        public List<IFormFile>? AdditionalImages { get; set; }

        public List<string>? RemovedImagesUrls { get; set; }

    }
}
