using FlightStatusManagement.Application.Common.Interfaces;
using FlightStatusManagement.Domain.Entities;
using MediatR;

namespace FlightStatusManagement.Application.Flights.Commands.CreateFlight
{
    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, int>
    {
        private readonly IApplicationDbContext _db;

        public CreateFlightCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> Handle(CreateFlightCommand request, CancellationToken ct)
        {
            var flight = new Flight
            {
                Origin = request.Origin,
                Destination = request.Destination,
                Departure = request.Departure,
                Arrival = request.Arrival,
                Status = request.Status
            };

            _db.Flights.Add(flight);

            await _db.SaveChangesAsync(ct);

            return flight.Id;
        }
    }
}
