using FluentAssertions;

namespace FlightStatusManagement.IntegrationTests.Tests
{
    public class AuthTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public AuthTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Login_ReturnsToken()
        {
            await _factory.Services.SeedUsersAsync();

            var client = _factory.CreateClient();

            var token = await TestAuth.LoginAndGetTokenAsync(client, "moderator_user", "12345");

            token.Should().NotBeNullOrWhiteSpace();
        }
    }
}
