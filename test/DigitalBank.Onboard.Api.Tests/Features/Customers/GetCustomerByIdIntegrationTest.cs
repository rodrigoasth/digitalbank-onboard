using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DigitalBank.Onboard.Api.Contracts;
using DigitalBank.Onboard.Api.Tests.Shared;
using Newtonsoft.Json;

namespace DigitalBank.Onboard.Api.Tests.Features.Customers
{
    public class GetCustomerByIdIntegrationTest : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;

        public GetCustomerByIdIntegrationTest(IntegrationTestFactory factory)
        {
            this._factory = factory;
        }

        [Fact]
        public async Task CreateCustomer_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v1/customers/6dec8fcd-ef3c-417f-bcd6-119d0a3a128f");

            //Assert
            var customer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
            Assert.NotNull(customer);
            Assert.NotNull(customer.PhoneNumber);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateCustomer_WithValidData_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v1/customers/abc123");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            
        }
    }
}