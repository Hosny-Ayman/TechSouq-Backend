using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Domain.Entities
{
    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public bool IsActive { get; set; } = true;

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Cart> carts {get;set;}

        public ICollection<Address> Addresses { get;set; }

        public ICollection<Order> Orders { get;set; }


        public ICollection<ProductReview> ProductReview { get; set; }
    }
  
    }
