using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductImageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> AddProductImage(ProductImage productImage)
        {
            _appDbContext.ProductImages.Add(productImage);
            var save = await _appDbContext.SaveChangesAsync();
            return save > 0 ? productImage.Id : 0;
        }

        public async Task<bool> DeleteProductImage(int productImageId)
        {
            return await _appDbContext.ProductImages.Where(x => x.Id == productImageId).ExecuteDeleteAsync() > 0;
        }

        public async Task<ProductImage> GetProductImage(int productImageId, bool trackingChanges = true)
        {
            var query = _appDbContext.ProductImages.AsQueryable();
            if (!trackingChanges) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id == productImageId);
        }

        public async Task<bool> UpdateProductImage(ProductImage productImage)
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsProductImageExists(int productImageId)
        {
            return await _appDbContext.ProductImages.AnyAsync(x => x.Id == productImageId);
        }
    }
}