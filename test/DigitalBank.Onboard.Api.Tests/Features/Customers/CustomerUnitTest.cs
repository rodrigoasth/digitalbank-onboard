using DigitalBank.Onboard.Api.Features.Customers;
using FluentAssertions;

namespace DigitalBank.Onboard.Api.Tests.Features.Customers
{
    public class CustomerUnitTest
    {
        [Fact]
        public void Customer_WhenDateOfBirthIsLessThan18YearsOld_ReturnsBrokenRules()
        {
            // Arrange
            var customer = new Customer
            {
                DateOfBirth = DateTime.Now.AddYears(-17)
            };

            // Act
            customer.Update();

            // Assert
            customer.RulesIsBroken().Should().BeTrue();
            customer.GetBrokenRules().Should().Be(Customer.Under18YearOldMessage);
        }
    }
}