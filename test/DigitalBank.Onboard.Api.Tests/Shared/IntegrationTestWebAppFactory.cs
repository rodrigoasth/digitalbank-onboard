
using Carter;
using DigitalBank.Onboard.Api.Infra.Respository;
using DigitalBank.Onboard.Database;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;

namespace DigitalBank.Onboard.Api.Tests.Shared
{
    public class IntegrationTestWebAppFactory
        : WebApplicationFactory<Program>,
          IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-CU18-ubuntu-20.04")
        .WithCleanUp(true)
        .Build();

        
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await _dbContainer.ExecScriptAsync(@"
                CREATE DATABASE DigitalBank;
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
                INSERT INTO UpdateCustomerCommand
                    (Id, FirstName, LastName, Email, DateOfBirth, PhoneNumber, Address, City, State, Country)
                VALUES
                    (NEWID(), 'John', 'Doe', 'john.doe@example.com', '1980-01-01', '1234567890', '123 Street', 'City', 'RJ', 'BR');
            ");
        }

        public new Task DisposeAsync() => _dbContainer.DisposeAsync().AsTask();
    }
}