using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Domain.Entities
{
    public class Coupons
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountValue { get; set; }
        public int DiscountType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApplicableOnDiscountedItems { get; set; }
        public int UsageLimit { get; set; }

    }
}
