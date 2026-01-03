namespace FlightStatusManagement.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Username { get; }
    }
}
