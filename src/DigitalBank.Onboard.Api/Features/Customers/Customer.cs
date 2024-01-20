
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Api.Shared.Domain;

namespace DigitalBank.Onboard.Api.Features.Customers
{
    public class Customer : Entity, IAggregateRoot
    {
        public const string Under18YearOldMessage = "Customer must be older than 18 years old";

        public Guid CustomerId { get; init;}
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime DateOfBirth { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string ZIPCode { get; init; }
        public string Country { get; init; }

        public Customer()
        {
            Under18YearsOld();
        }

        public void Update()
        {
            Under18YearsOld();
        }

        private void Under18YearsOld()
        {
            if (DateOfBirth.AddYears(18) > DateTime.Now)
                NotificationContext.AddNotification(Under18YearOldMessage);
        }        
    }    
}