namespace Customers.PhysicalPerson;

public class PhysicalCustomerResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Document { get; set; }
    public PersonType PersonType { get; set; } = PersonType.PhysicalPerson;
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZIPCode { get; set; }
    public string Country { get; set; }

    public PhysicalCustomerResponse(string firstName, string lastName, string document, DateTime dateOfBirth, string email, string phoneNumber, string address, string city, string state, string zIPCode, string country)
    {
        FirstName = firstName;
        LastName = lastName;
        Document = document;
        DateOfBirth = dateOfBirth;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        City = city;
        State = state;
        ZIPCode = zIPCode;
        Country = country;
    }

    
}