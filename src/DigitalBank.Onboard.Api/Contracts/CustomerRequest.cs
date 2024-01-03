
namespace DigitalBank.Onboard.Api.Contracts
{
    public class CustomerRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZIPCode { get; set; }
        public required string Country { get; set; }
    }
}