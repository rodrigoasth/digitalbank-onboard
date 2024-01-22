using DigitalBank.Onboard.Api.Features.Accounts;
using DigitalBank.Onboard.Api.Infra.Respository;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace DigitalBank.Onboard.Api.Tests.Features.Accounts
{
    public class BlockAccountUnitTest
    {
        [Fact]
        public async Task ShouldFinancialBlockAccount()
        {
            // Arrange
            var agency = 1;
            var accountNumber = 123456;
            var account = new Account(agency, accountNumber, Guid.NewGuid());
            var accountRepositoryMock = new Mock<IAccountRepository>();

            accountRepositoryMock
                .Setup(x => x.GetAccountAsync(agency, accountNumber))
                .ReturnsAsync(account);
            accountRepositoryMock
                .Setup(x => x.UpdateAccountAsync(account))
                .Returns(Task.CompletedTask);
                
            var validatorMock = new Mock<IValidator<BlockAccount.BlockAccountCommand>>();
            validatorMock
                .Setup(x => x.ValidateAsync(
                                It.IsAny<BlockAccount.BlockAccountCommand>(), 
                                It.IsAny<CancellationToken>()))                                
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
                
            var handler = new BlockAccount.Handler(validatorMock.Object, accountRepositoryMock.Object);

            var command = new BlockAccount.BlockAccountCommand { 
                                Agency = agency, 
                                AccountNumber = accountNumber,
                                BlockType = BlockAccount.BlockType.Financial };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            account.Status.Should().Be(AccountStatus.FinancialBlocked);
        }

        [Fact]
        public async Task ShouldJudicialBlockAccount()
        {
            // Arrange
            var agency = 1;
            var accountNumber = 123456;
            var account = new Account(agency, accountNumber, Guid.NewGuid());
            var accountRepositoryMock = new Mock<IAccountRepository>();

            accountRepositoryMock
                .Setup(x => x.GetAccountAsync(agency, accountNumber))
                .ReturnsAsync(account);
            accountRepositoryMock
                .Setup(x => x.UpdateAccountAsync(account))
                .Returns(Task.CompletedTask);
                
            var validatorMock = new Mock<IValidator<BlockAccount.BlockAccountCommand>>();
            validatorMock
                .Setup(x => x.ValidateAsync(
                                It.IsAny<BlockAccount.BlockAccountCommand>(), 
                                It.IsAny<CancellationToken>()))                                
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
                
            var handler = new BlockAccount.Handler(validatorMock.Object, accountRepositoryMock.Object);

            var command = new BlockAccount.BlockAccountCommand { 
                                Agency = agency, 
                                AccountNumber = accountNumber,
                                BlockType = BlockAccount.BlockType.Judicial };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            account.Status.Should().Be(AccountStatus.JudicialBlocked);
        }
    }
}