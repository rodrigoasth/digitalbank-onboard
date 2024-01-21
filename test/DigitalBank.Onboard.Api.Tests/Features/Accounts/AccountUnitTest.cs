using DigitalBank.Onboard.Api.Features.Accounts;
using FluentAssertions;

namespace DigitalBank.Onboard.Api.Tests.Features.Accounts
{
    public class AccountUnitTest
    {
        [Fact]
        public void Account_WhenAccountNumberLessThanSixDigits_ReturnsBrokenRules()
        {
            //Arrange
            var invalidAccountNumber = 12345;
            
            // Act
            var account = new Account(1, invalidAccountNumber, Guid.NewGuid());

            // Assert
            account.RulesIsBroken().Should().BeTrue();
            account.GetBrokenRules().Should().Be(Account.AccountNumberLessThanSixDigitsMessage);
        }        
    }
}