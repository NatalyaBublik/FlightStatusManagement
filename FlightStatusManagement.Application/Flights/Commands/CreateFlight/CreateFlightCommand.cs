using FlightStatusManagement.Domain.Enums;
using MediatR;

namespace FlightStatusManagement.Application.Flights.Commands.CreateFlight
{
    public record CreateFlightCommand(
        string Origin,
        string Destination,
        DateTimeOffset Departure,
        DateTimeOffset Arrival,
        FlightStatus Status
    ) : IRequest<int>;
}
