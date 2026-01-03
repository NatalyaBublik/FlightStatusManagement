using FluentValidation;

namespace FlightStatusManagement.Application.Flights.Commands.UpdateFlightStatus
{
    public class UpdateFlightStatusCommandValidator : AbstractValidator<UpdateFlightStatusCommand>
    {
        public UpdateFlightStatusCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
