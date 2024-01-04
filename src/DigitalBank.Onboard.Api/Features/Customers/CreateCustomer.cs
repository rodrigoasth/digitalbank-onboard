using Carter;
using DigitalBank.Onboard.Api.Contracts;
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Database;
using FluentValidation;
using Mapster;
using MediatR;

namespace DigitalBank.Onboard.Api.Features.Customers
{
    public static class CreateCustomer
    {
        public class CreateCustomerCommand : IRequest<Result<Guid>>
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
        }

        public class Validator : AbstractValidator<CreateCustomerCommand>
        {
            public Validator()
            {
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

        internal sealed class Handler : IRequestHandler<CreateCustomerCommand, Result<Guid>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IValidator<CreateCustomerCommand> _validator;

            public Handler(ApplicationDbContext dbContext, IValidator<CreateCustomerCommand> validator)
            {
                _dbContext = dbContext;
                _validator = validator;
            }

            public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<Guid>(new Error("Create customer validation", validationResult.ToString()));
                }

                var customer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    City = request.City,
                    State = request.State,
                    ZIPCode = request.ZIPCode,
                    Country = request.Country
                };

                if(customer.RulesIsBroken())
                    return Result.Failure<Guid>(new Error("Create customer check rules", customer.GetBrokenRules())); 


                _dbContext.Customers.Add(customer);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return customer.CustomerId;
            }
        }         
    } 

    public class CreateCustomerEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/customers", async (CustomerRequest request, ISender sender)=>{
                try
                {
                    var command = request.Adapt<CreateCustomer.CreateCustomerCommand>();
                    var result = await sender.Send(command);

                    if (result.IsFailure)                    {
                        return Results.BadRequest(result.Error);                    }

                    var customerId = result.Value;

                    return Results.Created($"/api/v1/customers/{customerId}", customerId);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }     
}