using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalBank.Onboard.Api.Infra;
using DigitalBank.Onboard.Api.Infra.Respository;
using DigitalBank.Onboard.Api.Shared;
using DigitalBank.Onboard.Database;
using FluentValidation;
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
            private readonly AccountRepository _accountRepository;

            public Handler(
                        IValidator<CreateAccountCommand> validator,
                        IAccountNumberRepository accountNumberRepository,
                        AccountRepository accountRepository)
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

                return Result.Success(account.AccountId);
        
            }
        }
    }
}