using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FlightStatusManagement.IntegrationTests.Tests
{
    public class FlightsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public FlightsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetFlights_WithoutToken_Returns401()
        {
            var resp = await _client.GetAsync("/api/flights");

            resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateFlight_AsUser_Returns403()
        {
            await TestDatabaseHelper.ResetDatabaseAsync(_factory.Services);

            await _factory.Services.SeedUsersAsync();

            var token = await TestAuth.LoginAndGetTokenAsync(_client, "user_user", "12345");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var resp = await _client.PostAsJsonAsync("/api/flights", new
            {
                origin = "ALA",
                destination = "AST",
                departure = "2026-01-10T08:30:00+06:00",
                arrival = "2026-01-10T11:15:00+06:00",
                status = "InTime"
            });

            resp.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateAndGetFlights_AsModerator_Works()
        {
            await TestDatabaseHelper.ResetDatabaseAsync(_factory.Services);

            await _factory.Services.SeedUsersAsync();

            var token = await TestAuth.LoginAndGetTokenAsync(_client, "moderator_user", "12345");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createResp = await _client.PostAsJsonAsync("/api/flights", new
            {
                origin = "ALA",
                destination = "AST",
                departure = "2026-01-10T08:30:00+06:00",
                arrival = "2026-01-10T11:15:00+06:00",
                status = "InTime"
            });

            createResp.StatusCode.Should().Be(HttpStatusCode.Created);

            var flights = await _client.GetFromJsonAsync<List<FlightDto>>("/api/flights");

            flights.Should().NotBeNull();

            flights!.Should().ContainSingle(f => f.Origin == "ALA" && f.Destination == "AST");
        }

        [Fact]
        public async Task ChangeFlightStatus_AsModerator_UpdatesStatus()
        {
            await TestDatabaseHelper.ResetDatabaseAsync(_factory.Services);

            await _factory.Services.SeedUsersAsync();

            var token = await TestAuth.LoginAndGetTokenAsync(_client, "moderator_user", "12345");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var create = await _client.PostAsJsonAsync("/api/flights", new
            {
                origin = "ALA",
                destination = "AST",
                departure = "2026-01-10T08:30:00+05:00",
                arrival = "2026-01-10T11:15:00+06:00",
                status = "InTime"
            });

            create.StatusCode.Should().Be(HttpStatusCode.Created);

            var flights = await _client.GetFromJsonAsync<List<FlightDto>>("/api/flights");

            var flightId = flights!.Single(f => f.Origin == "ALA" && f.Destination == "AST").Id;

            var patch = await _client.PatchAsJsonAsync($"/api/flights/{flightId}/status", new { status = "Delayed" });

            patch.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var flightsAfter = await _client.GetFromJsonAsync<List<FlightDto>>("/api/flights");

            flightsAfter!.Single(s => s.Id == flightId).Status.Should().Be("Delayed");
        }

        [Fact]
        public async Task ChangeFlightStatus_AsUser_Returns403()
        {
            await TestDatabaseHelper.ResetDatabaseAsync(_factory.Services);

            await _factory.Services.SeedUsersAsync();

            var token = await TestAuth.LoginAndGetTokenAsync(_client, "user_user", "12345");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var resp = await _client.PatchAsJsonAsync("/api/flights/1/status", new { status = "Cancelled" });

            resp.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        private record FlightDto(
            int Id,
            string Origin,
            string Destination,
            DateTimeOffset Departure,
            DateTimeOffset Arrival,
            string Status
        );
    }
}
