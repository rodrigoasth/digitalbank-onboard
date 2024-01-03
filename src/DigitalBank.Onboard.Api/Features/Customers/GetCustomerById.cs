using Carter;
using DigitalBank.Onboard.Api.Contracts;
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalBank.Onboard.Api.Features.Customers
{
    public static class GetCustomerById
    {
        public class GetCustomerByIdQuery : IRequest<Result<CustomerResponse>>
        {
            public Guid CustomerId { get; set; }
        }
        
        public class Validator : AbstractValidator<GetCustomerByIdQuery>
        {
            public Validator()
            {
                RuleFor(x => x.CustomerId).NotEmpty();
            }
        }
        
        internal sealed class Handler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerResponse>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IValidator<GetCustomerByIdQuery> _validator;
        
            public Handler(ApplicationDbContext dbContext, IValidator<GetCustomerByIdQuery> validator)
            {
                _dbContext = dbContext;
                _validator = validator;
            }
        
            public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                var customerResponse = await _dbContext.Customers
                    .Where(x => x.CustomerId == query.CustomerId)
                    .Select(x => new CustomerResponse
                    {
                        CustomerId = x.CustomerId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        DateOfBirth = x.DateOfBirth,
                        Address = x.Address,
                        City = x.City,
                        State = x.State,
                        ZIPCode = x.ZIPCode,
                        Country = x.Country
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                    if (customerResponse == null)
                    {
                        return Result.Failure<CustomerResponse>(new Error(
                            "GetCustomer.Null", 
                            "Customer not found by id specified"));
                    }

                    return customerResponse;                         
            }
        }        
    }

    public class GetCustomerByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/customers/{customerId:guid}", async (Guid customerId, ISender sender)=>{
                try
                {
                    var query = new GetCustomerById.GetCustomerByIdQuery
                    {
                        CustomerId = customerId
                    };

                    var result = await sender.Send(query);
    
                    if (result == null)
                        return Results.NotFound();
    
                    return Results.Ok(result.Value);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });
        }
    }
}