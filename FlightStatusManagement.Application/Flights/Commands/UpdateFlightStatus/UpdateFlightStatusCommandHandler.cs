using FlightStatusManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightStatusManagement.Application.Flights.Commands.UpdateFlightStatus
{
    public class UpdateFlightStatusCommandHandler : IRequestHandler<UpdateFlightStatusCommand>
    {
        private readonly IApplicationDbContext _db;

        public UpdateFlightStatusCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Handle(UpdateFlightStatusCommand request, CancellationToken ct)
        {
            var flight = await _db.Flights.SingleOrDefaultAsync(f => f.Id == request.Id, ct);
            
            if (flight is null)
                throw new KeyNotFoundException($"Flight with id {request.Id} not found.");

            flight.Status = request.Status;

            await _db.SaveChangesAsync(ct);
        }
    }
}
