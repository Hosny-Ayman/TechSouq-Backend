using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Domain.Enums
{
    public enum CartStatus
    {
        Active=1,
        CheckedOut=2,
        Cancelled=3
    }

    public enum DiscountType
    {
        Percentage = 1, 
        FixedAmount = 2 
    }
}
