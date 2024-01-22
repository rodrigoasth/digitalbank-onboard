
namespace DigitalBank.Onboard.Api.Contracts
{
    public class AccountRequest
    {
        public Guid CustomerId { get; set; }
        public int Agency { get; set; }
    }
}