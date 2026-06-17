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
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Queries
{
    public class DeliveryZonesQuery: IDeliveryZonesQuery
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;


        public DeliveryZonesQuery(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<PagedResponse<DeliveryZoneDto>> GetAllDeliveryZonesPaged(int pageNumber, int pageSize, string? NameSearch)
        {
            var query = _appDbContext.DeliveryZones.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(NameSearch))
            {
                query = query.Where(x => x.Name.Contains(NameSearch));
            }

            var TotalRecords = await query.CountAsync();


            var data = await query.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var AllData = _mapper.Map<List<DeliveryZoneDto>>(data);

            return new PagedResponse<DeliveryZoneDto>(AllData, TotalRecords, pageNumber, pageSize);
        }
    }


}
