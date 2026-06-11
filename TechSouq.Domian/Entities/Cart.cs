using TechSouq.Domain.Enums;

namespace TechSouq.Domain.Entities
{
    public class Cart
    {

        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public SystemEnums Status { get; set; } = SystemEnums.Active;

        public ICollection<CartItem> CartItems { get; set; }
    }
}
