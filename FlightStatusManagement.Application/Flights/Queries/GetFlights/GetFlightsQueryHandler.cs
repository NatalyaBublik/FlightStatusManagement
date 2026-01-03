using FlightStatusManagement.Application.Common.Interfaces;
using FlightStatusManagement.Application.Flights.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightStatusManagement.Application.Flights.Queries.GetFlights
{
    public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, IReadOnlyList<FlightDto>>
    {
        private readonly IApplicationDbContext _db;

        public GetFlightsQueryHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<FlightDto>> Handle(GetFlightsQuery request, CancellationToken ct)
        {
            var flights = _db.Flights.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Origin))
            {
                flights = flights.Where(f => f.Origin == request.Origin);
            }

            if (!string.IsNullOrWhiteSpace(request.Destination))
            {
                flights = flights.Where(f => f.Destination == request.Destination);
            }

            return await flights
                .OrderBy(f => f.Arrival)
                .Select(f => new FlightDto(
                    f.Id, f.Origin, f.Destination, f.Departure, f.Arrival, f.Status))
                .ToListAsync(ct);
        }
    }
}
