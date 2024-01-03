namespace Customers.PhysicalPerson;
using System;
using System.ComponentModel.DataAnnotations;

public class PhysicalCustomerRequest
{
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

    public PhysicalCustomerRequest(string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber, string address, string city, string state, string zipCode, string country)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        City = city;
        State = state;
        ZIPCode = zipCode;
        Country = country;
    }
}

