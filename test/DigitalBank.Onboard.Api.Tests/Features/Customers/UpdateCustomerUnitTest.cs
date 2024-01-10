using DigitalBank.Onboard.Api.Features.Customers;
using DigitalBank.Onboard.Api.Infra.Respository;
using DigitalBank.Onboard.Database;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using static DigitalBank.Onboard.Api.Features.Customers.UpdateCustomer;

namespace DigitalBank.Onboard.Api.Tests.Features.Customers
{
    public class UpdateCustomerUnitTests 
    {
        [Trait("Category","Unit")]               
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var repositoryMock = new Mock<ICustomerRepository>();
            var validatorMock = new Mock<IValidator<UpdateCustomerCommand>>();
            var command = new UpdateCustomerCommand
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZIPCode = "12345",
                Country = "USA"
            };
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new UpdateCustomer.Handler(repositoryMock.Object, validatorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(command.CustomerId);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFailureResult()
        {
            // Arrange
            var repositoryMock = new Mock<ICustomerRepository>();
            var validatorMock = new Mock<IValidator<UpdateCustomerCommand>>();
            var command = new UpdateCustomerCommand();
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> 
                            { 
                                new FluentValidation.Results.ValidationFailure("FirstName", "First name is required") 
                            }));

            var handler = new UpdateCustomer.Handler(repositoryMock.Object, validatorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_CustomerUnder18YearsOld_ReturnsFailureResult()
        {
            // Arrange
            var repositoryMock = new Mock<ICustomerRepository>();
            var validatorMock = new Mock<IValidator<UpdateCustomerCommand>>();
            var command = new UpdateCustomerCommand
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(2010, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "12345678901",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZIPCode = "12345",
                Country = "USA"
            };
            
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new UpdateCustomer.Handler(repositoryMock.Object, validatorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Validator_ValidatesRequiredFields()
        {
            // Arrange
            var validator = new UpdateCustomer.Validator();
            var command = new UpdateCustomerCommand
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "12345678901",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZIPCode = "12345",
                Country = "USA"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_InvalidatesMissingFirstName()
        {
            // Arrange
            var validator = new UpdateCustomer.Validator();
            var command = new UpdateCustomerCommand
            {
                CustomerId = Guid.NewGuid(),
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "12345678901",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZIPCode = "12345",
                Country = "USA"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "FirstName" && e.ErrorMessage == "'First Name' must not be empty.");
        }
    }
}