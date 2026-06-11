using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Queries
{
    public class CategorieQuery : ICategorieQuery
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CategorieQuery(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<CategorieSelectorDto>> GetAllCategorieForSelect()
        {
           return await _appDbContext.Categories.AsNoTracking().Select(x => new CategorieSelectorDto
            {
                Id = x.Id,
                Name = x.Name

            }).ToListAsync();
        }

        public async Task<PagedResponse<CategorieDto>> GetAllCategoriePaged(int pageNumber, int pageSize)
        {
            var query = _appDbContext.Categories.AsNoTracking();

            var TotalRecords = await query.CountAsync();

            var data = await query.OrderBy(x=>x.Id).Skip((pageNumber - 1)*pageSize).Take(pageSize).ToListAsync();

            var AllData = _mapper.Map<List<CategorieDto>>(data);

            return new PagedResponse<CategorieDto>(AllData, TotalRecords, pageNumber, pageSize);

        }

        
    }
}
