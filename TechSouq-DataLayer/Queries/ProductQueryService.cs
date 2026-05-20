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
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace TechSouq.Infrastructure.Queries
{
    public class ProductQueryService : IProductQueryService
    {

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
       

        public ProductQueryService(AppDbContext context, IMapper mapper)
        {
            _appDbContext = context;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ProductDto>> GetProductsPaged(int pageNumber, int pageSize, string? searchTerm = null,string? Catogrie = null)
        {
            var totalRecords = await _appDbContext.Products.CountAsync();

            var query = _appDbContext.Products.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            if(!string.IsNullOrWhiteSpace(Catogrie))
            {
                var catogrie = await _appDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Name == Catogrie);
                if(catogrie != null)
                {
                    query = query.Where(p => p.CategoryId==catogrie.Id);
                }
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
                    Price = p.Price,
                    FirstImage = p.ProductImages.Select(img => img.ImageUrl).FirstOrDefault(),
                    Stock = p.Stock,
                   PriceAfterDiscount = p.PriceAfterDiscount,
                   DiscountStartDate = p.DiscountStartDate,
                   DiscountEndDate = p.DiscountEndDate,
                   AverageRating = p.AverageRating,
                   TotalReviews = p.TotalReviews,
                   IsFreeShipping = p.IsFreeShipping,




                }).ToListAsync();

            return new PagedResponse<ProductDto>(data, totalRecords, pageNumber, pageSize);
        }

        public async Task<ProductDto> GetProductById(int productId)
        {

            var data = await _appDbContext.Products
        .AsNoTracking()
        .Where(x => x.Id == productId)
        .Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock,

            Images = x.ProductImages
                .Select(img => img.ImageUrl)
                .ToList(),

            FirstImage = x.ProductImages
                .Select(img => img.ImageUrl)
                .FirstOrDefault(),

            AverageRating = x.AverageRating,

            TotalReviews = x.TotalReviews,

            IsFreeShipping = x.IsFreeShipping,

            DiscountStartDate = x.DiscountStartDate, 

            DiscountEndDate = x.DiscountEndDate,

            PriceAfterDiscount = x.PriceAfterDiscount
        })
        .FirstOrDefaultAsync();

          

            return data;

        }

    }
}
