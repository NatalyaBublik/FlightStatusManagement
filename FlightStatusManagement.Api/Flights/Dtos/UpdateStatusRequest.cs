using FlightStatusManagement.Domain.Enums;

namespace FlightStatusManagement.Api.Flights.Dtos
{   
    public record UpdateStatusRequest(FlightStatus Status);
}
