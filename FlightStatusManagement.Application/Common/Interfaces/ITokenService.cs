using FlightStatusManagement.Domain.Entities;


namespace FlightStatusManagement.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
    }
}
