using AutoMapper;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace TechSouq.Infrastructure.Queries
{

    
    public class CouponsQuery: ICouponsQuery
    {

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CouponsQuery(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<PagedResponse<CouponSummaryDto>> GetAllCouponsPaged(int pageNumber, int pageSize, string? CodeSearch)
        {
            var query = _appDbContext.Coupons.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(CodeSearch))
            {
                query = query.Where(x => x.Code.Contains(CodeSearch));
            }

            var TotalRecords = await query.CountAsync();

            var data = await query.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new CouponSummaryDto
            {

                Id = x.Id,
                Code = x.Code,
                IsActive = x.IsActive

            }).ToListAsync();


            return new PagedResponse<CouponSummaryDto>(data, TotalRecords, pageNumber, pageSize);
        }

        public async Task<CouponDto?> GetCouponById(int id)
        {
            return await _appDbContext.Coupons.AsNoTracking().Where(y=>y.Id == id).ProjectTo<CouponDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);

         
                    
        }
    }
}
