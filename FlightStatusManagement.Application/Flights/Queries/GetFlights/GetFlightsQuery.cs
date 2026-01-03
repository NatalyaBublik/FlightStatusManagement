using FlightStatusManagement.Application.Flights.Dtos;
using MediatR;

namespace FlightStatusManagement.Application.Flights.Queries.GetFlights
{
    public record GetFlightsQuery(string? Origin, string? Destination) : IRequest<IReadOnlyList<FlightDto>>;
}
