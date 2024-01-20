using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalBank.Onboard.Api.Features.Customers;
using DigitalBank.Onboard.Api.Infra.Respository;
using FluentAssertions;
using FluentValidation;
using Moq;
using static DigitalBank.Onboard.Api.Features.Customers.CreateCustomer;

namespace DigitalBank.Onboard.Api.Tests.Features.Customers
{
    public class CreateCustomerUnitTest
    {
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var repositoryMock = new Mock<ICustomerRepository>();
            var validatorMock = new Mock<IValidator<CreateCustomerCommand>>();
            var command = new CreateCustomerCommand
            {
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

            var handler = new CreateCustomer.Handler(repositoryMock.Object, validatorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_InvalidPhone_ReturnsFailureResult()
        {
            // Arrange
            var repositoryMock = new Mock<ICustomerRepository>();
            var validatorMock = new Mock<IValidator<CreateCustomerCommand>>();
            var command = new CreateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZIPCode = "12345",
                Country = "USA"
            };

            var validationFailures = new List<FluentValidation.Results.ValidationFailure>
            {                
                new FluentValidation.Results.ValidationFailure("PhoneNumber", "PhoneNumber is required"),
            };

            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationFailures));

            var handler = new CreateCustomer.Handler(repositoryMock.Object, validatorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Message.Should().Be("PhoneNumber is required");

        }
    }
}