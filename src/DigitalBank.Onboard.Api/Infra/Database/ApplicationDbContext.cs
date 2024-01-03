using DigitalBank.Onboard.Api.Features.Customers;
using Microsoft.EntityFrameworkCore;

namespace DigitalBank.Onboard.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new CustomerConfiguration());            
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
