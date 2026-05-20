using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Enums;

namespace TechSouq.Application.Dtos
{
    public class GetCouponDto
    {
        public string Code { get; set; }
        public decimal DiscountValue { get; set; }
        public DiscountType DiscountType { get; set; }
        public bool IsApplicableOnDiscountedItems { get; set; }
    }
}
