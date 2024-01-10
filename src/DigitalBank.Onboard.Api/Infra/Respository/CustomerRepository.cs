using DigitalBank.Onboard.Api.Features.Customers;
using DigitalBank.Onboard.Database;
using Microsoft.EntityFrameworkCore;

namespace DigitalBank.Onboard.Api.Infra.Respository
{
    public interface ICustomerRepository
    {
        Task<Guid> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task<Customer?> GetAsync(Guid customerId);
    }
    
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddAsync(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return customer.CustomerId;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Customer?> GetAsync(Guid customerId)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }        
    }
}