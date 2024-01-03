using System;
namespace Customers.LegalPerson;

public class LegalCustomerRequest
{
    public string LegalName { get;}
    public string Email { get;}
    public string PhoneNumber { get;}
    public string Address { get;}
    public string City { get;}
    public string State { get;}
    public string ZIPCode { get;}
    public string Country { get;}

    public LegalCustomerRequest(string legalName, string email, string phoneNumber, string address, string city, string state, string zipCode, string country)
    {
        LegalName = legalName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        City = city;
        State = state;
        ZIPCode = zipCode;
        Country = country;
    }
   
}

