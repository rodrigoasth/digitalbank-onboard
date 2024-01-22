
using static DigitalBank.Onboard.Api.Features.Accounts.BlockAccount;

namespace DigitalBank.Onboard.Api.Contracts
{
    public class BlockAccountRequest
    {
        public int Agency { get; set; }
        public int AccountNumber { get; set; }
        public BlockType BlockType { get; set; }        
    }
}