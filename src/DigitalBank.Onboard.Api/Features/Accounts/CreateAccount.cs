using Carter;
using DigitalBank.Onboard.Api.Contracts;
using DigitalBank.Onboard.Api.Infra.Respository;
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Database;
using FluentValidation;
using Mapster;
using MediatR;

namespace DigitalBank.Onboard.Api.Features.Accounts
{
    public static class CreateAccount
    {
        public class CreateAccountCommand : IRequest<Result<Guid>>
        {
            public Guid CustomerId { get; set; }
            public int Agency { get; set; }
        }
        
        public class Validator : AbstractValidator<CreateAccountCommand>
        {
            public Validator()
            {
                RuleFor(x => x.CustomerId).NotEmpty();
                RuleFor(x => x.Agency).NotEmpty().GreaterThan(0);
            }
        }
        
        internal sealed class Handler : IRequestHandler<CreateAccountCommand, Result<Guid>>
        {
            private readonly IValidator<CreateAccountCommand> _validator;
            private readonly IAccountNumberRepository _accountNumberRepository;
            private readonly IAccountRepository _accountRepository;
            private readonly object _bus;

            public Handler(
                        IValidator<CreateAccountCommand> validator,
                        IAccountNumberRepository accountNumberRepository,
                        IAccountRepository accountRepository)
            {
                _validator = validator;
                _accountNumberRepository = accountNumberRepository;
                _accountRepository = accountRepository;
            }
        
            public async Task<Result<Guid>> Handle(
                                                CreateAccountCommand command, 
                                                CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<Guid>(new Error("Create account validation", validationResult.ToString()));
                }

                var accountNumber = _accountNumberRepository.GetNextAccountNumberAvailable(command.Agency);
                var account = new Account(command.Agency, accountNumber, command.CustomerId);

                if(account.RulesIsBroken())
                    return Result.Failure<Guid>(new Error("Create account check rules", account.GetBrokenRules()));

                await _accountRepository.CreateAccountAsync(account);  

                //publish a message to service bus queue
                await _bus.PublishAsync(new AccountCreatedEvent(account.AccountId));


                return Result.Success(account.AccountId);
        
            }
        }        
    }

    public class CreateAccountEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/accounts", async (
                                                AccountRequest request, 
                                                ISender sender)=>{
                try
                {
                    var command = request.Adapt<CreateAccount.CreateAccountCommand>();
                    var result = await sender.Send(command);
    
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }
    
                    var accountId = result.Value;

                    return Results.Created($"/api/v1/customers/{accountId}", accountId);
    
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}