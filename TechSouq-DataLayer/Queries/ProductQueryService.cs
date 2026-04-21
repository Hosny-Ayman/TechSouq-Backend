using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Infrastructure.Data;
using TechSouq.Application.Helper;
using TechSouq.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using TechSouq.Application.Queries;

namespace TechSouq.Infrastructure.Queries
{
    public class ProductQueryService : IProductQueryService
    {

        private readonly AppDbContext _appDbContext;

        public ProductQueryService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<PagedResponse<ProductDto>> GetProductsPaged(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var totalRecords = await _appDbContext.Products.CountAsync();

            var query = _appDbContext.Products.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            var data = await query
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    //Description = p.Description,
                    //Stock = p.Stock,
                    Price = p.Price,
                    FirstImage = p.ProductImages.Select(img => img.ImageUrl).FirstOrDefault(),
                    //CategoryName = p.Categorie.Name,
                    //BrandName = p.Brand.Name,



                }).ToListAsync();

            return new PagedResponse<ProductDto>(data, totalRecords, pageNumber, pageSize);
        }

        public async Task<ProductDto> GetProductById(int productId)
        {

            var data = await _appDbContext.Products.AsNoTracking().Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                Stock = x.Stock,
                FirstImage = x.ProductImages.Select(img => img.ImageUrl).FirstOrDefault(),
                Images = x.ProductImages.Select(x=>x.ImageUrl).ToList(),
                CategoryName = x.Categorie.Name,
                BrandName = x.Brand.Name,
            }).FirstOrDefaultAsync(x=>x.Id == productId);

            return data;

        }

       

       

    }
}
