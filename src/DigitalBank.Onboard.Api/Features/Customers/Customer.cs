
using DigitalBank.Onboard.Api.Shared.Domain;

namespace DigitalBank.Onboard.Api.Features.Customers
{
    public class Customer : Entity
    {
        public const string Under18YearOldMessage = "Customer must be older than 18 years old";

        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIPCode { get; set; }
        public string Country { get; set; }

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