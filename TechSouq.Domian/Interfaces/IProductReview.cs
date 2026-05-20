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

        Task<bool> CanUserReviewProductAsync(int userId, int productId);

        Task<int?> CanUserEditHisReview(int productId, int userId);


        //Task<List<ProductReview>> GetAllReviewsForProductPaged(page);
    }
}
