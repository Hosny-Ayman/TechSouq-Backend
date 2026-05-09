namespace TechSouq.Application.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Phone { get; set; }

        public int UserId { get; set; }

        public string? building {  get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public bool Active { get; set; } = true;

        public string Email { get; set; }
    }

}
