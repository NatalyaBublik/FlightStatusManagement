using FlightStatusManagement.Domain.Enums;

namespace FlightStatusManagement.Application.Flights.Dtos
{
    public record FlightDto(
        int Id,
        string Origin,
        string Destination,
        DateTimeOffset Departure,
        DateTimeOffset Arrival,
        FlightStatus Status
    );
}