using System.Net;
using System.Text;
using DigitalBank.Onboard.Api.Contracts;
using DigitalBank.Onboard.Api.Tests.Shared;
using Newtonsoft.Json;

namespace DigitalBank.Onboard.Api.Tests.Features.Accounts;
public class CreateAccountIntegrationTest: IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;

    public CreateAccountIntegrationTest(IntegrationTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAccount_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new AccountRequest
        {
            CustomerId = Guid.NewGuid(),
            Agency = 1
        };

        var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"api/v1/accounts", body);

        // Assert that return 201 hpttp status code
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task CreateAccount_WithInvalidAgency_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new AccountRequest
        {
            CustomerId = Guid.NewGuid(),
            Agency = 0
        };

        var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"api/v1/accounts", body);

        // Assert that return 400 hpttp status code
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

