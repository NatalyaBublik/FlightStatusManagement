using FluentValidation;

namespace FlightStatusManagement.Application.Flights.Queries.GetFlights
{
    public class GetFlightsQueryValidator : AbstractValidator<GetFlightsQuery>
    {
        public GetFlightsQueryValidator()
        {
            RuleFor(x => x.Origin).MaximumLength(256);
            RuleFor(x => x.Destination).MaximumLength(256);
        }
    }
}