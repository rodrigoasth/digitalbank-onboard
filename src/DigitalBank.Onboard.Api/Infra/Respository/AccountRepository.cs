using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalBank.Onboard.Api.Features.Accounts;
using DigitalBank.Onboard.Database;

namespace DigitalBank.Onboard.Api.Infra.Respository
{
    public interface IAccountRepository
    {        
        Task CreateAccountAsync(Account account);
        Task<Account> GetAccountAsync(int agency, int accountNumber);
        Task UpdateAccountAsync(Account account);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAccountAsync(Account account)
        {
            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccountAsync(
                                        int agency, 
                                        int accountNumber)
        {
            var account = await _dbContext.Accounts.FindAsync(agency, accountNumber);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            return account;
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}