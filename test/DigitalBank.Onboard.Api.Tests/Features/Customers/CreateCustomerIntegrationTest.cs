using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DigitalBank.Onboard.Api.Tests.Shared;
using Newtonsoft.Json;
using static DigitalBank.Onboard.Api.Features.Customers.CreateCustomer;

namespace DigitalBank.Onboard.Api.Tests.Features.Customers
{
    public class CreateCustomerIntegrationTest : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;

        public CreateCustomerIntegrationTest(IntegrationTestFactory factory)
        {
            this._factory = factory;
        }

        [Fact]
        public async Task CreateCustomer_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new CreateCustomerCommand
            {    
                FirstName = "Teste",
                Email = "rodrigoasth@gmail.com",
                LastName = "Teste",
                DateOfBirth = DateTime.Now.AddYears(-18),
                PhoneNumber = "11999999999",
                Address = "Rua Teste",
                City = "São Paulo",
                State = "SP",
                ZIPCode = "00000000",
                Country = "BR"
            };

            var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"api/v1/customers", body);

            // Assert that return 201 hpttp status code
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
            
        }

        [Fact]
        public async Task CreateCustomer_WithInvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new CreateCustomerCommand
            {    
                FirstName = "Teste",
                Email = "",
                LastName = "Teste",
                DateOfBirth = DateTime.Now.AddYears(-18),
                PhoneNumber = "11999999999",
                Address = "Rua Teste",
                City = "São Paulo",
                State = "SP",
                ZIPCode = "00000000",
                Country = "BR"
            };

            var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"api/v1/customers", body);

            // Assert that return 400 hpttp status code
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
    }
}