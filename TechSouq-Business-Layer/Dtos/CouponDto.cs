using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Enums;

namespace TechSouq.Application.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public decimal DiscountValue { get; set; }

        public DiscountType DiscountType { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsApplicableOnDiscountedItems { get; set; }

        public int UsageLimit { get; set; }
    }
}
