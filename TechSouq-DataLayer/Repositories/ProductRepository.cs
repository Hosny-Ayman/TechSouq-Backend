using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> AddProduct(Product product)
        {
            _appDbContext.Products.Add(product);
            var save = await _appDbContext.SaveChangesAsync();
            return save > 0 ? product.Id : 0;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            return await _appDbContext.Products.Where(x => x.Id == productId).ExecuteDeleteAsync() > 0;
        }

        public async Task<Product> GetProduct(int productId, bool trackingChanges = true)
        {
            var query = _appDbContext.Products.AsQueryable();

            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsProductExists(int productId)
        {
            return await _appDbContext.Products.AnyAsync(x => x.Id == productId);
        }

        public async Task<int> RemoveAllExpiredDiscountsAsync()
        {
            int updatedRows = await _appDbContext.Products
         .Where(p => p.DiscountEndDate != null && p.DiscountEndDate <= DateTime.UtcNow)
         .ExecuteUpdateAsync(setters => setters
             .SetProperty(p => p.PriceAfterDiscount, p => null)
             .SetProperty(p => p.DiscountStartDate, p => null)
             .SetProperty(p => p.DiscountEndDate, p => null)
         );

            return updatedRows;
        }

       
    }
}