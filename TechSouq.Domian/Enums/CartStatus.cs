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

    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }

    //public enum PaymentMethod
    //{
    //    Cash = 1,
    //    Card = 2,
    //    PayPal = 3,
    //    Stripe = 4,
    //    ApplePay = 5,
    //    GooglePay = 6,
    //    VodafoneCash = 7,
    //    InstaPay = 8,
    //    BankTransfer = 9,
    //    Wallet = 10
    //}
}
