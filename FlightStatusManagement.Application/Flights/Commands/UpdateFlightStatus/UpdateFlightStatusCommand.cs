using FlightStatusManagement.Domain.Enums;
using MediatR;

namespace FlightStatusManagement.Application.Flights.Commands.UpdateFlightStatus
{
    public record UpdateFlightStatusCommand(int Id, FlightStatus Status) : IRequest;
}
