
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Api.Shared.Domain;

namespace DigitalBank.Onboard.Api.Features.Accounts
{
    public class Account : Entity, IAggregateRoot
    {
        public const string AccountNumberLessThanSixDigitsMessage = "Account number must be 6 digits";
        public const string Is_Already_Financial_Blocked_Message = "Account is already financial blocked";
        public const string Is_Already_Judicial_Blocked_Message = "Account is already judicial blocked";

        public Guid AccountId { get; set; }
        public int Agency { get; set; }
        public int AccountNumber { get; set; }
        public Guid CustomerId { get; set; }   
        public AccountStatus Status { get; set; } 
        public AccountType Type { get; set; }

        public Account(
                int agency, 
                int accountNumber, 
                Guid customerId, 
                AccountType? type = AccountType.CheckingAccount)
        {
            int digitsAccountNumber = (int)Math.Floor(Math.Log10(accountNumber) + 1);
            if(digitsAccountNumber != 6)
            {
                NotificationContext.AddNotification(AccountNumberLessThanSixDigitsMessage);
                return;
            }

            AccountId = Guid.NewGuid();
            Agency = agency;
            AccountNumber = accountNumber;
            CustomerId = customerId;
            Status = AccountStatus.Active;
        } 

        public void FinancialBlockAccount()
        {
            if(Status == AccountStatus.FinancialBlocked){            
                NotificationContext.AddNotification(Is_Already_Financial_Blocked_Message);
                return;
            }             

            Status = AccountStatus.FinancialBlocked;
        } 

        public void JudicialBlockAccount()
        {
            if(Status == AccountStatus.JudicialBlocked){            
                NotificationContext.AddNotification(Is_Already_Judicial_Blocked_Message);
                return;
            }             

            Status = AccountStatus.JudicialBlocked;
        }
    } 

    public enum AccountStatus{
        Active = 1,
        FinancialBlocked = 2,
        JudicialBlocked = 3
    } 

    public enum AccountType{
        CheckingAccount = 1,
        SavingsAccount = 2,
        SalaryAccount = 3,
        StudentAccount = 4,
        CommercialAccount = 5,
        JointCheckingAccount = 6        
    }  
}