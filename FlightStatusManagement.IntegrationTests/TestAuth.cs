using System.Net.Http.Json;
using FlightStatusManagement.Application.Auth.Login;

namespace FlightStatusManagement.IntegrationTests
{
    public static class TestAuth
    {
        public static async Task<string> LoginAndGetTokenAsync(HttpClient client, string username, string password)
        {
            var resp = await client.PostAsJsonAsync("/api/auth/login", new { username, password });

            resp.EnsureSuccessStatusCode();

            var body = await resp.Content.ReadFromJsonAsync<LoginResponse>();

            return body!.AccessToken;
        }
    }
}
