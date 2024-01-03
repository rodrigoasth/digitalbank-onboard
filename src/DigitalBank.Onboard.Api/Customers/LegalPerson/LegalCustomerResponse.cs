namespace Customers.LegalPerson;
public class LegalCustomerResponse
{
    public string LegalName { get; }
    public PersonType PersonType { get; } = PersonType.LegalPerson;
    public string Document { get; set; }
    public string Email { get; }

    public string PhoneNumber { get; }

    public string Address { get; }

    public string City { get; }

    public string State { get; }

    public string ZIPCode { get; }

    public string Country { get; }


    public LegalCustomerResponse(string legalName, string document, string email, string phoneNumber, string address, string city, string state, string zIPCode, string country)
    {
        LegalName = legalName;
        Document = document;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        City = city;
        State = state;
        ZIPCode = zIPCode;
        Country = country;
    }

    
}