using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Infrastructure.Data;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace TechSouq.Infrastructure.Queries
{
    public class BrandQuery : IBrandQuery
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public BrandQuery(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<PagedResponse<BrandDto>> GetAllBrandsPaged(int pageNumber, int pageSize)
        {
            var query = _appDbContext.Brands.AsNoTracking();

            var TotalRecords = await query.CountAsync();

            var data = await query.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var AllData = _mapper.Map<List<BrandDto>>(data);

            return new PagedResponse<BrandDto>(AllData, TotalRecords, pageNumber, pageSize);
        }


    }
       

        
}
