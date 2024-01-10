using System.Net;
using System.Text;
using DigitalBank.Onboard.Api.Tests.Shared;
using Newtonsoft.Json;
using static DigitalBank.Onboard.Api.Features.Customers.UpdateCustomer;

namespace DigitalBank.Onboard.Api.Tests.Features.Customers
{
    public class UpdateCustomerIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly IntegrationTestWebAppFactory _factory;

        public UpdateCustomerIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            this._factory = factory;
        }

        [Fact]
        public async Task UpdateCustomer_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new UpdateCustomerCommand
            {    
                CustomerId = Guid.NewGuid(),            
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
            var response = await client.PutAsync($"api/v1/customers/{command.CustomerId}", body);

            // Assert that return 204 hpttp status code
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
        }

    }
}