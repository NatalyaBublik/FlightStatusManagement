namespace FlightStatusManagement.Application.Auth.Login
{
    public record LoginResponse(string AccessToken, int ExpiresInSeconds, string Role);
}
