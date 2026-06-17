using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.interfaces;

namespace TechSouq.Application.Queries
{
    public interface IDashboardQuery
    {
        public Task<DashboardDto> ShowDashboardInfo();
       
        public Task <PagedResponse<RecentOrderDto>> RecentSales(IRecentSaleceQueryParams Params);

        public Task<List<decimal>> SalesLast7Days();

        public Task<List<BestSellingProductsDto>> BestSellingProducts();

    }
}
