using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application.interfaces
{
    public class IRecentSaleceQueryParams
    {

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public string? SortField { get; set; }

        public int? SortOrder { get; set; }
    }
}
