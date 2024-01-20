
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Api.Shared.Domain;

namespace DigitalBank.Onboard.Api.Features.Accounts
{
    public class Account : Entity, IAggregateRoot
    {
        public Guid AccountId { get; set; }
        public int Agency { get; set; }
        public int AccountNumber { get; set; }
        public Guid CustomerId { get; set; }   
        public bool IsBlocked { get; set; } 

        public Account(int agency, int accountNumber, Guid customerId)
        {
            if(accountNumber != 6)
            {
                NotificationContext.AddNotification("Account number must be 6 digits");
                return;
            }

            AccountId = Guid.NewGuid();
            Agency = agency;
            AccountNumber = accountNumber;
            CustomerId = customerId;
        } 

        public void BlockAccount()
        {
            if(IsBlocked)
            {
                NotificationContext.AddNotification("Account is already blocked");
                return;
            }

            IsBlocked = true;
        } 
    }    
}