using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;

namespace TechSouq.Application.Queries
{
    public interface IBrandQuery
    {
        Task<PagedResponse<BrandDto>> GetAllBrandsPaged(int pageNumber, int pageSize);

    }
}
