using DigitalBank.Onboard.Api.Features.Customers;
using DigitalBank.Onboard.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace DigitalBank.Onboard.Api.Tests.Shared
{
    public class IntegrationTestFactory
        : WebApplicationFactory<Program>,
          IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
                                        //.WithImage("mcr.microsoft.com/mssql/server:2019-CU18-ubuntu-20.04")
                                        //.WithCleanUp(true)
                                        .Build();

        override protected void ConfigureWebHost(IWebHostBuilder builder)
        {
            
            var connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {   services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            });

            //builder.UseEnvironment("Development");

            /*
            _dbContainer.ExecScriptAsync(@"
                CREATE DATABASE DigitalBank;              
            ");

            _dbContainer.ExecScriptAsync(@"
                CREATE TABLE Customers
                (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    FirstName NVARCHAR(50),
                    LastName NVARCHAR(50),
                    Email NVARCHAR(255),
                    DateOfBirth DATE,
                    PhoneNumber NVARCHAR(50),
                    Address NVARCHAR(50),
                    City NVARCHAR(50),
                    State NVARCHAR(50),
                    Country NVARCHAR(50)                    
                );
            ");

            _dbContainer.ExecScriptAsync(@"
                INSERT INTO Customers
                    (Id, FirstName, LastName, Email, DateOfBirth, PhoneNumber, Address, City, State, Country)
                VALUES
                    ('6dec8fcd-ef3c-417f-bcd6-119d0a3a128f', 'John', 'Doe', 'john.doe@example.com', '1980-01-01', '1234567890', '123 Street', 'City', 'RJ', 'BR');
            ");
            */

        }
        
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();  

            using (var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<ApplicationDbContext>();

                await cntx.Database.EnsureCreatedAsync();

                cntx.Customers.Add(new Customer
                {
                    CustomerId = new Guid("6dec8fcd-ef3c-417f-bcd6-119d0a3a128f"),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    PhoneNumber = "1234567890",
                    Address = "123 Street",
                    City = "City",
                    State = "RJ",
                    Country = "BR",
                    ZIPCode = "00000000" 
                });
                
                await cntx.SaveChangesAsync();
            }      
        }
        
        public async Task DisposeAsync() => await _dbContainer.StopAsync();
    }
}