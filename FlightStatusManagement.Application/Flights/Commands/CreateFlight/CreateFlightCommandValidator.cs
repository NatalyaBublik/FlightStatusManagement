using FluentValidation;

namespace FlightStatusManagement.Application.Flights.Commands.CreateFlight
{
    public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
    {
        public CreateFlightCommandValidator()
        {
            RuleFor(x => x.Origin).NotEmpty().MaximumLength(256);

            RuleFor(x => x.Destination).NotEmpty().MaximumLength(256);

            RuleFor(x => x.Arrival)
                .Must((cmd, arrival) => arrival > cmd.Departure)
                .WithMessage("Arrival must be later than Departure.");

            RuleFor(x => x.Status).IsInEnum();
        }
    }
}