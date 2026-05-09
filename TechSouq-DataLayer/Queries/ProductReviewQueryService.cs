using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AutoMapper.QueryableExtensions;

namespace TechSouq.Infrastructure.Queries
{
    public class ProductReviewQueryService:IProductReviewQueryService
    {

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;


        public ProductReviewQueryService(AppDbContext context , IMapper mapper)
        {
            _appDbContext = context;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ProductReviewDto>> GetAllReviewsPagedAsync(int pageNumber, int pageSize, int productId)
        {

            var query = _appDbContext.ProductReview
        .AsNoTracking()
        .Where(x => x.ProductId == productId);

            var totalRecords = await query.CountAsync();

            var reviews = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ProductReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResponse<ProductReviewDto>(
                reviews, totalRecords, pageNumber, pageSize);

        }
    }
}
