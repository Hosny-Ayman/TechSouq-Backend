using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<int> AddProduct(Product product)
        {
            _appDbContext.Products.Add(product);
            var save = await _appDbContext.SaveChangesAsync();
            return save > 0 ? product.Id : 0;
        }

        public async Task<bool> DeleteProduct(int productId, List<string> imagesToRemove)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                var affectedRows = await _appDbContext.Products.Where(x => x.Id == productId).ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsDeleted, true));

                if (affectedRows <= 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }


                if(imagesToRemove!=null&& imagesToRemove.Count>0)
                {
                    await _appDbContext.ProductImages.Where(x => x.ProductId == productId && imagesToRemove.Contains(x.ImageUrl)).ExecuteDeleteAsync();
                }

                await transaction.CommitAsync();
                return true;

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
           
        }

        public async Task<Product> GetProduct(int productId, bool trackingChanges = true)
        {
            var query = _appDbContext.Products.AsQueryable();

            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.Include(p => p.ProductImages).FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<bool> UpdateProduct(Product product,bool?IsTraking=true)
        {
            if(IsTraking == true)
            {
                return await _appDbContext.SaveChangesAsync() > 0;
            }
            else
            {
                var OldProduct = await _appDbContext.Products.FirstOrDefaultAsync(x=>x.Id == product.Id);

                _mapper.Map(product, OldProduct);

                return await _appDbContext.SaveChangesAsync() > 0;
            }

            
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

        public void RemvoeProductImages(List<ProductImage> imagesToDelete)
        {
            //if(productId!=null)
            //{
            //    await _appDbContext.ProductImages.Where(x => x.ProductId == productId && ImagesUrl.Contains(x.ImageUrl)).ExecuteDeleteAsync();

            //    return true;
            //}

            //return false;

            _appDbContext.ProductImages.RemoveRange(imagesToDelete);
        }
    }
}