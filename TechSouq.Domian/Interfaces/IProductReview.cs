using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Domain.Interfaces
{
    public interface IProductReview
    {
        Task<int> AddReviewAsync(ProductReview ProductReview);

        Task<bool> UpdateReviewAsync(ProductReview ProductReview);

       

        //Task<List<ProductReview>> GetAllReviewsForProductPaged(page);
    }
}
