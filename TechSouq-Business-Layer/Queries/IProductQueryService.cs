using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Helper;
using TechSouq.Application.Dtos;


namespace TechSouq.Application.Queries
{
    public interface  IProductQueryService
    {
        public Task<PagedResponse<ProductDto>> GetProductsPaged(int pageNumber, int pageSize,string? searchTerm=null, string? Catogrie = null);

        public Task<ProductDto> GetProductById(int productId);

    }


}
