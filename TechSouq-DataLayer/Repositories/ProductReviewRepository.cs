using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TechSouq.Infrastructure.Repositories
{
    public class ProductReviewRepository : IProductReview
    {
        private readonly AppDbContext _context;
        public ProductReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddReviewAsync(ProductReview productReview)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            productReview.CreatedAt = DateTime.UtcNow;
            _context.ProductReview.Add(productReview);

            var eff = await _context.SaveChangesAsync();

            if (eff == 0)
            {
                transaction.Rollback();
                return 0;
            }
                

            await _context.Products
                .Where(p => p.Id == productReview.ProductId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.TotalReviews, p => p.TotalReviews + 1)
                    .SetProperty(p => p.AverageRating,
                        p => ((p.AverageRating * p.TotalReviews) + productReview.Rating)
                             / (p.TotalReviews + 1)
                    )
                );

            await transaction.CommitAsync();

            return productReview.Id;
        }

        public async Task<bool> UpdateReviewAsync(ProductReview productReview)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var review = await _context.ProductReview
                .FirstOrDefaultAsync(x => x.Id == productReview.Id);

            if (review == null || review.ProductId != productReview.ProductId)
            {
                await transaction.RollbackAsync();
                return false;
            }
                  

            var oldRating = review.Rating;

            _context.Entry(review).CurrentValues.SetValues(productReview);
            review.UpdatedAt = DateTime.UtcNow;

            if(oldRating != productReview.Rating)
            {
                await _context.Products
               .Where(p => p.Id == review.ProductId)
               .ExecuteUpdateAsync(s => s
                   .SetProperty(p => p.AverageRating,
                       p => p.TotalReviews == 0 ? 0 :
                       ((p.AverageRating * p.TotalReviews) - oldRating + productReview.Rating)
                       / p.TotalReviews
                   )
               );
            }
           

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }
    }
}
