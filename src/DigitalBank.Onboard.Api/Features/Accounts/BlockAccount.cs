

using Carter;
using DigitalBank.Onboard.Api.Contracts;
using DigitalBank.Onboard.Api.Infra.Respository;
using DigitalBank.Onboard.Api.Shared;
using FluentValidation;
using Mapster;
using MediatR;

namespace DigitalBank.Onboard.Api.Features.Accounts
{
    public static class BlockAccount
    {
        public enum BlockType {
            Financial = 1,
            Judicial = 2
        }

        public class BlockAccountCommand : IRequest<Result<int>>
        {
            public int Agency { get; set; }
            public int AccountNumber { get; set; }
            public BlockType BlockType { get; set; }
        }
        
        public class Validator : AbstractValidator<BlockAccountCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Agency).NotEmpty().GreaterThan(0);
                RuleFor(x => x.AccountNumber).NotEmpty();
            }
        }
        
        internal sealed class Handler : IRequestHandler<BlockAccountCommand, Result>
        {
            private readonly IValidator<BlockAccountCommand> _validator;
            private readonly IAccountRepository _accountRepository;

            public Handler(
                    IValidator<BlockAccountCommand> validator, 
                    IAccountRepository accountRepository)
            {
                _validator = validator;
                _accountRepository = accountRepository;
            }
        
            public async Task<Result> Handle(
                                            BlockAccountCommand command, 
                                            CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(
                                                            command, 
                                                            cancellationToken);
                if (!validationResult.IsValid)
                    return Result.Failure<int>(new Error("Block Account validation", validationResult.ToString()
                                                    ));

                var account = await _accountRepository.GetAccountAsync(
                                                            command.Agency, 
                                                            command.AccountNumber);
                
                switch(command.BlockType)
                {
                    case BlockType.Financial:
                        account.FinancialBlockAccount();
                        break;
                    case BlockType.Judicial:
                        account.JudicialBlockAccount();
                        break;
                }

                if(account.RulesIsBroken())
                    return Result.Failure<Guid>(new Error("Block Account check rules", account.GetBrokenRules()));
                
                await _accountRepository.UpdateAccountAsync(account);

                return Result.Success();        
            }
        }
    }

    public class BlockAccountEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/v1/customers", async (BlockAccountRequest request, ISender sender)=>{
                try
                {
                    var command = request.Adapt<BlockAccount.BlockAccountCommand>();
                    var result = await sender.Send(command);
    
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }
    
                    var resultValue = result.Value;
    
                    return Results.Ok(resultValue)                    ;
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}