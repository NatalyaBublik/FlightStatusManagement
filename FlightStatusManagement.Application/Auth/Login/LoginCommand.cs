using MediatR;

namespace FlightStatusManagement.Application.Auth.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
}
