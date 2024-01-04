using System.Runtime.CompilerServices;
using Carter;
using DigitalBank.Onboard.Api.Contracts;
using DigitalBank.Onboard.Api.Infra.Respository;
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Database;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DigitalBank.Onboard.Api.Features.Customers.UpdateCustomer;

namespace DigitalBank.Onboard.Api.Features.Customers 
{
    public static class UpdateCustomer 
    {
        public class UpdateCustomerCommand : IRequest<Result<Guid>>
        {
            public Guid CustomerId { get; set; }
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
        }
        
        public class Validator : AbstractValidator<UpdateCustomerCommand>
        {
            public Validator()
            {
                RuleFor(x => x.CustomerId).NotEmpty();
                RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3);
                RuleFor(x => x.LastName).NotEmpty().MinimumLength(3);                
                RuleFor(x => x.DateOfBirth).NotEmpty().LessThan(DateTime.Now);
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\d{11}$");
                RuleFor(x => x.City).NotEmpty().MinimumLength(3);
                RuleFor(x => x.State).NotEmpty().MaximumLength(2);
                RuleFor(x => x.Country).NotEmpty().MinimumLength(2);                
            }
        }
        
        internal sealed class Handler : IRequestHandler<UpdateCustomerCommand, Result<Guid>>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IValidator<UpdateCustomerCommand> _validator;
        
            public Handler(ICustomerRepository customerRepository, IValidator<UpdateCustomerCommand> validator)
            {
                _customerRepository = customerRepository;
                _validator = validator;
            }
        
            public async Task<Result<Guid>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    return Result.Failure<Guid>(new Error("Customer updated succesfully.", validationResult.ToString()));

                var incoming = command.Adapt<Customer>();
                incoming.Update();

                if(incoming.RulesIsBroken())
                    return Result.Failure<Guid>(new Error("Customer updated failed.", incoming.GetBrokenRules()));

                var customer = await _customerRepository.GetAsync(incoming.CustomerId);

                if (customer != null)
                    await _customerRepository.UpdateAsync(incoming);

                return incoming.CustomerId;     
            }
        }
    }

    public class UpdateCustomerEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/v1/customers/{customerId}", async (Guid customerId, CustomerRequest request, ISender sender)=>{
                try
                {
                    var command = request.Adapt<UpdateCustomerCommand>();
                    command.CustomerId = customerId;
                    
                    var result = await sender.Send(command);
    
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }
    
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}