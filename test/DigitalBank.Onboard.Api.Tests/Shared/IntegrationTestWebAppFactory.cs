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
        .WithImage("mcr.microsoft.com/mssql/server:2019-CU18-ubuntu-20.04")
        .WithCleanUp(true)
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

            builder.UseEnvironment("Development");            
        }
        
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await _dbContainer.ExecScriptAsync(@"
                CREATE DATABASE DigitalBank;              
            ");
            await _dbContainer.ExecScriptAsync(@"
                CREATE TABLE Customers
                (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    FirstName NVARCHAR(50),
                    LastName NVARCHAR(50),
                    Email NVARCHAR(255),
                    DateOfBirth DATE
                    PhoneNumber NVARCHAR(50),
                    Address NVARCHAR(50),
                    City NVARCHAR(50),
                    State NVARCHAR(50),
                    Country NVARCHAR(50),
                    
                );
            ");

            await _dbContainer.ExecScriptAsync(@"
                INSERT INTO UpdateCustomerCommand
                    (Id, FirstName, LastName, Email, DateOfBirth, PhoneNumber, Address, City, State, Country)
                VALUES
                    ('6dec8fcd-ef3c-417f-bcd6-119d0a3a128f', 'John', 'Doe', 'john.doe@example.com', '1980-01-01', '1234567890', '123 Street', 'City', 'RJ', 'BR');
            ");
        }
        
        public new Task DisposeAsync() => _dbContainer.DisposeAsync().AsTask();
    }
}