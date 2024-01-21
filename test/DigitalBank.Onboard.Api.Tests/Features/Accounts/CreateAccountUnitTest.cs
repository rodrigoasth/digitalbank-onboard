using DigitalBank.Onboard.Api.Features.Accounts;
using DigitalBank.Onboard.Api.Infra.Respository;
using FluentAssertions;
using FluentValidation;
using Moq;
using static DigitalBank.Onboard.Api.Features.Accounts.CreateAccount;

namespace DigitalBank.Onboard.Api.Tests.Features.Accounts;
public class CreateAccountUnitTest
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var accountRepositoryMock = new Mock<IAccountRepository>();
        var accountNumberRepositoryMock = new Mock<IAccountNumberRepository>();
        var validatorMock = new Mock<IValidator<CreateAccountCommand>>();
        var command = new CreateAccountCommand
        {
            Agency = 1,
            CustomerId = Guid.NewGuid()
        };
        
        validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        accountNumberRepositoryMock
            .Setup(a => a.GetNextAccountNumberAvailable(command.Agency))
            .Returns(123456);

        var handler = new CreateAccount.Handler(validatorMock.Object, accountNumberRepositoryMock.Object, accountRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_InvalidAgency_ReturnsFailureResult()
    {
        // Arrange
        var accountRepositoryMock = new Mock<IAccountRepository>();
        var accountNumberRepositoryMock = new Mock<IAccountNumberRepository>();
        var validatorMock = new Mock<IValidator<CreateAccountCommand>>();
        var invalidAgency = -1;
        var command = new CreateAccountCommand
        {
            Agency = invalidAgency,
            CustomerId = Guid.NewGuid()
        };
        
        validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var handler = new CreateAccount.Handler(validatorMock.Object, accountNumberRepositoryMock.Object, accountRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}
